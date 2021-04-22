﻿/*
 * Copyright (c) Chase Roth <cxr6988@rit.edu>
 * Licensed under the MIT License. See repository root directory for more info.
*/

using System;
using System.Runtime.InteropServices;
using HWND = System.IntPtr; // A handle to a window
using HHOOK = System.IntPtr; // A HANDLE
using WPARAM = System.IntPtr; // A UINT_PTR which varies in respect to target architecture (x86, x64)
using LPARAM = System.IntPtr; // A LONG_PTR which varies in respect to target architecture (x86, x64)
using HOOKPROC = System.IntPtr;
using HINSTANCE = System.IntPtr;
using DWORD = System.UInt32;
using LRESULT = System.IntPtr; // A LONG_PTR which varies in respect to target architecture (x86, x64)

/* Reference Source: https://docs.microsoft.com/en-us/windows/win32/winprog/windows-data-types
 * Type Rules:
 * HWND -> IntPtr
 * LParam -> IntPtr
 * WParam -> IntPtr, would use UIntPtr but it is not CLS compliant
 * DWORD -> UInt32
 * HHOOK -> IntPtr
 * HOOKPROC -> IntPtr
 * HINSTANCE -> IntPtr
 * LRESULT -> IntPtr
*/

/// <summary>
/// Contains external functions defined in windows native libraries.
/// Read Source: https://docs.microsoft.com/en-us/windows/win32/winmsg/about-hooks
/// </summary>
namespace SharpHotkeys.Native
{
    /// <summary>
    /// Contains imported user32.dll registration functions.
    /// </summary>
    internal static class NativeAPI
    {
        /// <summary>
        /// User32 DLL that contains these external functions.
        /// </summary>
        const string USER_32 = "user32";

        // Read Source: https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-registerhotkey
        [DllImport(USER_32, SetLastError = true)]
        public static extern bool RegisterHotKey(   
            HWND hWnd, 
            int id, 
            uint fsModifiers,
            uint vk
        );

        // Read Source: https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-unregisterhotkey
        [DllImport(USER_32, SetLastError = true)]
        public static extern bool UnregisterHotKey(
            HWND hWnd,
            int id
        );

        // Read Source: https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-callnexthookex
        [DllImport(USER_32)]
        public static extern IntPtr CallNextHookEx(
          HHOOK hhk,
          int nCode,
          WPARAM wParam,
          LPARAM lParam
        );

        // Read Source: https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-setwindowshookexa
        [DllImport(USER_32)]
        public static extern HHOOK SetWindowsHookExA(
            int idHook,
            HOOKPROC lpfn, // Pass method address callback function here for intercepting hooks
            HINSTANCE hmod,
            DWORD dwThreadId
        );

        // Read Source: https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-unhookwindowshookex
        [DllImport(USER_32)]
        public static extern bool UnhookWindowsHookEx(
          HHOOK hhk
        );

        // https://docs.microsoft.com/en-us/windows/win32/api/errhandlingapi/nf-errhandlingapi-getlasterror
        // _Post_equals_last_error_ DWORD GetLastError();
    }

    internal class Delegates
    {
        /// <summary>
        /// Defines the delegate of the callback function for hooks that is given to the <see cref="NativeAPI.SetWindowsHookExA(int, HWND, HWND, DWORD)"/> function.
        /// </summary>
        /// <param name="keyId">Key unique identifier given when the hot-key was registered.</param>
        /// <param name="wParam"></param>
        /// <param name="lParam"></param>
        /// <returns></returns>
        public delegate LRESULT HookCallbackDelegate(in int keyId, in WPARAM wParam, in LPARAM lParam);
    }
}
