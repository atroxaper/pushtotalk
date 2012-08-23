public class HotKeyForm: System.Windows.Forms.Form
{
	[System.Flags()]
		public enum KeyModifiers
	{
		None = 0,
		Alt = 1,    
		Control = 2,    
		Shift = 4,    
		Windows = 8
	}

	[System.Runtime.InteropServices.DllImport("user32.dll", SetLastError=true, EntryPoint="RegisterHotKey")]
	private static extern bool User32_RegisterHotKey(System.IntPtr hWnd, int id, KeyModifiers fsModifiers, uint vk);
        
	[System.Runtime.InteropServices.DllImport("user32.dll", SetLastError=true, EntryPoint="UnregisterHotKey")]
	private static extern bool User32_UnregisterHotKey(System.IntPtr hWnd, int id);
	
	private bool hotKeySet = false;
	public bool HotKeySet
	{
		get { return this.hotKeySet; }
	}

	public void RegisterHotKey(KeyModifiers keyModifiers, uint key)
	{
		if(hotKeySet)
			UnregisterHotKey();
      
		hotKeySet = User32_RegisterHotKey(Handle, 100, keyModifiers, key);
	}

	public void UnregisterHotKey()
	{
		if(hotKeySet)
			hotKeySet = !User32_UnregisterHotKey(Handle, 100);
	}

	protected override void Dispose(bool disposing)
	{
		UnregisterHotKey();
		base.Dispose (disposing);
	}

	public event System.EventHandler HotKeyPress;

	protected virtual void OnHotKeyPress(object sender, System.EventArgs e)
	{
        // Intentionally left empty.
    }

	protected override void WndProc(ref System.Windows.Forms.Message m)
	{
		const int WM_HOTKEY = 0x0312;
		switch(m.Msg)    
		{
			case WM_HOTKEY:
				System.EventArgs e;
				e = new System.EventArgs();
				if(HotKeyPress!=null)
					HotKeyPress(this, e);
				OnHotKeyPress(this, e);
				break;    
		}
		base.WndProc(ref m);
	}
}
