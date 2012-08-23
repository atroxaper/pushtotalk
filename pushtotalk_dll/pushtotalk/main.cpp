#include <mmdeviceapi.h>
#include <endpointvolume.h>
#include <Functiondiscoverykeys_devpkey.h>

unsigned int CurrentCaptureDeviceIndex = 0xFFFF;
HRESULT hr;
unsigned int uiNumCaptureDevices;

#define SAFE_RELEASE(punk)  \
	if ((punk) != NULL)  \
{ (punk)->Release(); (punk) = NULL; }

#define FUNCTIONALITY_GETMUTESTATUS 1
#define FUNCTIONALITY_MUTE 2
#define FUNCTIONALITY_UNMUTE 3

enum functionality
{
	GetStatus,
	Mute,
	UnMute
};

int HandleMic(int Index, functionality f, BOOL* MuteStatus)
{
	if (Index < 0)
		return 1;
	
	if ((GetStatus == f) && (NULL == MuteStatus))
		return 2;

	CoInitialize(NULL);
	IAudioEndpointVolume *endpointVolume = NULL;
	IMMDeviceEnumerator *deviceEnumerator = NULL;

	hr = CoCreateInstance(__uuidof(MMDeviceEnumerator), NULL, CLSCTX_INPROC_SERVER, __uuidof(IMMDeviceEnumerator), (LPVOID *)&deviceEnumerator);
	IMMDeviceCollection* ppDevices;
	hr = deviceEnumerator->EnumAudioEndpoints(eCapture, DEVICE_STATE_ACTIVE, &ppDevices);

	IMMDevice* pEndpoint = NULL;
	IPropertyStore* pProps = NULL;
	hr = ppDevices->Item(Index, &pEndpoint);
	hr = pEndpoint->Activate(__uuidof(IAudioEndpointVolume), CLSCTX_INPROC_SERVER, NULL, (LPVOID *)&endpointVolume);

	 if (GetStatus == f)
		hr = endpointVolume->GetMute(MuteStatus);
	else 
		hr = endpointVolume->SetMute(Mute == f, NULL);

	SAFE_RELEASE(pProps);
	SAFE_RELEASE(pEndpoint);
	SAFE_RELEASE(ppDevices);
	SAFE_RELEASE(endpointVolume);
	CoUninitialize();

	return 0;
}

extern "C"
{
	__declspec(dllexport) BOOL GetMuteStatus(int Index)
	{
		BOOL bIsMuted = false;  
		if (HandleMic(Index, GetStatus, &bIsMuted) != 0)
		{
			// Error handling in a DLL?
		}
		return bIsMuted;
	}
}

extern "C"
{
	__declspec(dllexport) void MuteMic(int Index)
	{
		HandleMic(Index, Mute, NULL);
	}
}

extern "C"
{
	__declspec(dllexport) void UnMuteMic(int Index)
	{
		HandleMic(Index, UnMute, NULL);
	}
}
int GetCaptureDeviceName(wchar_t* CaptureDevice, unsigned int CaptureDeviceLen, unsigned int Index)
{
	CoInitialize(NULL);
	IMMDeviceEnumerator *deviceEnumerator = NULL;
	hr = CoCreateInstance(__uuidof(MMDeviceEnumerator), NULL, CLSCTX_INPROC_SERVER, __uuidof(IMMDeviceEnumerator), (LPVOID *)&deviceEnumerator);
	IMMDeviceCollection* ppDevices;
	hr = deviceEnumerator->EnumAudioEndpoints(eCapture, DEVICE_STATE_ACTIVE, &ppDevices);
	hr = ppDevices->GetCount(&uiNumCaptureDevices);

	IMMDevice* pEndpoint = NULL;
	//LPWSTR pwszID = NULL;
	IPropertyStore* pProps = NULL;
	hr = ppDevices->Item(Index, &pEndpoint);
	//pEndpoint->GetId(&pwszID);

	pEndpoint->OpenPropertyStore(STGM_READ, &pProps);
	PROPVARIANT varName;
	//PROPVARIANT varDeviceDesc;

	PropVariantInit(&varName);

	// Get the endpoint's friendly-name property.
	hr = pProps->GetValue(PKEY_Device_FriendlyName, &varName);
	//hr = pProps->GetValue(PKEY_Device_DeviceDesc, &varDeviceDesc); // Skip this?

	wcscpy_s(CaptureDevice, CaptureDeviceLen, varName.pwszVal);

	//CoTaskMemFree(pwszID);
	//pwszID = NULL;
	PropVariantClear(&varName);
	SAFE_RELEASE(pProps);
	SAFE_RELEASE(pEndpoint);
	SAFE_RELEASE(ppDevices);

	return 0;
}

extern "C"
{
	__declspec(dllexport) bool FindFirstCaptureDevice(wchar_t* FirstCaptureDevice, unsigned int FirstCaptureDeviceLen)
	{
		CurrentCaptureDeviceIndex = 0;
		GetCaptureDeviceName(FirstCaptureDevice, FirstCaptureDeviceLen, CurrentCaptureDeviceIndex);

		return true;
	}
}

extern "C"
{
	__declspec(dllexport) bool FindNextCaptureDevice(wchar_t* CaptureDevice, unsigned int CaptureDeviceLen)
	{
		CurrentCaptureDeviceIndex++;
		if (CurrentCaptureDeviceIndex >= uiNumCaptureDevices)
			return false; // No more items

		GetCaptureDeviceName(CaptureDevice, CaptureDeviceLen, CurrentCaptureDeviceIndex);

		return true;
	}
}
