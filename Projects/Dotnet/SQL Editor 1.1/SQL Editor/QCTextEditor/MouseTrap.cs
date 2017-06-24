using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Reflection;

namespace QueryCommander.Editor
{
	/// <summary>
	/// Summary description for MouseTrap.
	/// </summary>
	public class MouseTrap
	{
		//struct for POINT
		[StructLayout(LayoutKind.Sequential)]
		private class POINT 
		{
			public int x;
			public int y;
		}

		/* C MouseLLHookStruct declared as: 
		typedef struct 
		{
		POINT pt;
		DWORD mouseData;
		DWORD flags;
		DWORD time;
		ULONG_PTR dwExtraInfo;
		}
		*/
		[StructLayout(LayoutKind.Sequential)]
		private class MouseLLHookStruct
		{
			public POINT pt;
			public uint mouseData;
			public uint flags;
			public uint time;
			public UIntPtr dwExtraInfo;
		}

		//imports SetWindowsHookEx function from user32.dll
		[DllImport("user32.dll",CharSet=CharSet.Auto, CallingConvention=CallingConvention.StdCall)]
		private static extern int SetWindowsHookEx(int idHook, HookProc lpfn, 
			IntPtr hInstance, int threadId);

		//imports UnhookWindowsHookEx function from user32.dll
		[DllImport("user32.dll",CharSet=CharSet.Auto, CallingConvention=CallingConvention.StdCall)]
		private static extern bool UnhookWindowsHookEx(int idHook);

		//imports CallNextHookEx function from user32.dll
		[DllImport("user32.dll",CharSet=CharSet.Auto, CallingConvention=CallingConvention.StdCall)]
		private static extern int CallNextHookEx(int idHook, int nCode, IntPtr wParam, IntPtr lParam); 

		//declare hook handle
		static int hHook = 0;

		//decalre const for mouse messages
		private const int WH_MOUSE = 7;
		private const int WH_MOUSE_LL = 14;

		//declare callback delegate
		private delegate int HookProc(int nCode, IntPtr wParam, IntPtr lParam);

		//declare MouseHookProcedure as HookProc type
		HookProc MouseHookProcedure; 

		public MouseTrap()
		{
			//create a new instance of HookProc
			MouseHookProcedure = new HookProc(MouseTrap.MouseHookProc);

			//set-up the callback for mouse messages, to call our MouseHookProcedure routine
			hHook = SetWindowsHookEx(WH_MOUSE_LL, MouseHookProcedure, (IntPtr) 
				Marshal.GetHINSTANCE(Assembly.GetExecutingAssembly().GetModules()[0]).ToInt32(), 0);

			//if we failed...
			if(hHook == 0)
			{
				//alert the user
				return;
			}
		}
		public void Unload()
		{
			UnhookWindowsHookEx(hHook);
		}
		private static int MouseHookProc(int nCode, IntPtr wParam, IntPtr lParam)
		{
			//extract our mouse data by casting to a MouseLLHookStruct
			MouseLLHookStruct mhsData = (MouseLLHookStruct) Marshal.PtrToStructure(lParam, typeof(MouseLLHookStruct));

			//if it's not for us...
			if (nCode < 0)
			{
				//return the value of the next assigned hook
				return CallNextHookEx(hHook, nCode, wParam, lParam);
			}
				//otherwise...
			else
			{
				//output our co-ords
				xPos=mhsData.pt.x;
				yPos=mhsData.pt.y;
//				SubclassedForm.textBox1.Text = "Mouse is at: " + 
//					mhsData.pt.x.ToString() + "," +
//					mhsData.pt.y.ToString();

				//return the value of the next assigned hook
				return CallNextHookEx(hHook, nCode, wParam, lParam); 
			}
		}
		public static int xPos;
		public static int yPos;


	}
	public class MouseMoveEventArgs: EventArgs 
	{
		public MouseMoveEventArgs(int x, int y) 
		{
			this.X=x;
			this.Y=y;
		}
		public int X;
		public int Y;
	}
}
