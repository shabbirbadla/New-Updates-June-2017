using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace Udyog.Application.License
{
    class dll
    {
        [DllImport("advapi32.dll", SetLastError = true)]
        public static extern bool GetCurrentHwProfile(IntPtr lpHwProfileInfo);
    }

    public class HWProfile
    {
        [Flags]
        public enum DockInfo
        {
            DOCKINFO_DOCKED = 0x02,
            DOCKINFO_UNDOCKED = 0x01,
            DOCKINFO_USER_SUPPLIED = 0x04,
            DOCKINFO_USER_DOCKED = 0x05,
            DOCKINFO_USER_UNDOCKED = 0x06
        }

        [StructLayout(LayoutKind.Sequential)]
        public class HW_PROFILE_INFO
        {
            [MarshalAs(UnmanagedType.U4)]
            public Int32 dwDockInfo;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 39)]
            public string szHwProfileGuid;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
            public string szHwProfileName;
        }
        static public HW_PROFILE_INFO GetCurrent()
        {
            HW_PROFILE_INFO profile;
            IntPtr profilePtr = IntPtr.Zero;

            try
            {
                // Allocate a HW_PROFILE_INFO in unmanaged memory and get a pointer to it.
                profile = new HW_PROFILE_INFO();
                profilePtr = Marshal.AllocHGlobal(Marshal.SizeOf(profile));
                Marshal.StructureToPtr(profile, profilePtr, false);



                // Call GetCurrentHwProfile to populate the HW_PROFILE_INFO struct.
                if (!dll.GetCurrentHwProfile(profilePtr))
                {
                    throw new Exception("Call to GetCurrentHwProfile failed");
                }
                else
                {
                    Marshal.PtrToStructure(profilePtr, profile);
                    return profile;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.ToString());
            }
            finally
            {
                if (profilePtr != IntPtr.Zero) Marshal.FreeHGlobal(profilePtr);
            }
        }
    }
}
