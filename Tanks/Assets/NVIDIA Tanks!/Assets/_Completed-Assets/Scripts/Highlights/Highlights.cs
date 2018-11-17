/*
* Copyright (c) 2018, NVIDIA CORPORATION.  All rights reserved.
*
* NVIDIA CORPORATION and its licensors retain all intellectual property
* and proprietary rights in and to this software, related documentation
* and any modifications thereto.  Any use, reproduction, disclosure or
* distribution of this software and related documentation without an express
* license agreement from NVIDIA CORPORATION is strictly prohibited.
*/

using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using System.Text;
using System;

namespace NVIDIA {	
	public class Highlights {

		public static bool instanceCreated = false;

		public enum HighlightScope
		{
			Highlights					= 0x00,
			HighlightsRecordVideo		= 0x01,
			HighlightsRecordScreenshot  = 0x02,
            Ops                         = 0x03,
			MAX
		}

		public enum HighlightType {
            None        = 0x00,
			Milestone 	= 0x01,
			Achievement = 0x02,
			Incident 	= 0x04,
			StateChange = 0x08,
			Unannounced = 0x10,
			MAX 		= 0x20
		};

		public enum HighlightSignificance {
            None            = 0x00,
			ExtremelyBad 	= 0x01,
			VeryBad 		= 0x02,
			Bad 			= 0x04,
			Neutral 		= 0x10,
			Good 			= 0x100,
			VeryGood 		= 0x200,
			ExtremelyGood 	= 0x400,
			MAX 			= 0x800
		};

		public enum Permission {
			Granted = 0,
			Denied 	= 1,
			MustAsk = 2,
			Unknown = 3,
			MAX 	= 4,
		};

        public enum ReturnCode
        {
            SUCCESS = 0,
            SUCCESS_VERSION_OLD_SDK = 1001,
            SUCCESS_VERSION_OLD_GFE = 1002,
            SUCCESS_PENDING = 1003,
            SUCCESS_USER_NOT_INTERESTED = 1004,
            SUCCESS_PERMISSION_GRANTED = 1005,

            ERR_GENERIC = -1001,
            ERR_GFE_VERSION = -1002,
            ERR_SDK_VERSION = -1003,
            ERR_NOT_IMPLEMENTED = -1004,
            ERR_INVALID_PARAMETER = -1005,
            ERR_NOT_SET = -1006,
            ERR_SHADOWPLAY_IR_DISABLED = -1007,
            ERR_SDK_IN_USE = -1008,
            ERR_GROUP_NOT_FOUND = -1009,
            ERR_FILE_NOT_FOUND = -1010,
            ERR_HIGHLIGHTS_SETUP_FAILED = -1011,
            ERR_HIGHLIGHTS_NOT_CONFIGURED = -1012,
            ERR_HIGHLIGHTS_SAVE_FAILED = -1013,
            ERR_UNEXPECTED_EXCEPTION = -1014,
            ERR_NO_HIGHLIGHTS = -1015,
            ERR_NO_CONNECTION = -1016,
            ERR_PERMISSION_NOT_GRANTED = -1017,
            ERR_PERMISSION_DENIED = -1018,
            ERR_INVALID_HANDLE = -1019,
            ERR_UNHANDLED_EXCEPTION = -1020,
            ERR_OUT_OF_MEMORY = -1021,
            ERR_LOAD_LIBRARY = -1022,
            ERR_LIB_CALL_FAILED = -1023,
            ERR_IPC_FAILED = -1024,
            ERR_CONNECTION = -1025,
            ERR_MODULE_NOT_LOADED = -1026,
            ERR_LIB_CALL_TIMEOUT = -1027,
            ERR_APPLICATION_LOOKUP_FAILED = -1028,
            ERR_APPLICATION_NOT_KNOWN = -1029,
            ERR_FEATURE_DISABLED = -1030,
            ERR_APP_NO_OPTIMIZATION = -1031,
            ERR_APP_SETTINGS_READ = -1032,
            ERR_APP_SETTINGS_WRITE = -1033,
        };

		public struct TranslationEntry {
			public TranslationEntry(string _Language, string _Translation){
				Language = _Language;
				Translation = _Translation;
			}
			public string Language;
			public string Translation;
		};

		[StructLayout(LayoutKind.Sequential), Serializable]
		private struct Scope {
			[MarshalAsAttribute(UnmanagedType.SysInt)]
			public int value;
		};

		[StructLayout(LayoutKind.Sequential),Serializable]
		private struct HighlightDefinitionInternal {
			[MarshalAsAttribute(UnmanagedType.LPStr)]
			public string id;
			[MarshalAsAttribute(UnmanagedType.I1)]
			public bool userDefaultInterest;
			[MarshalAsAttribute(UnmanagedType.SysInt)]
			public int highlightTags;
			[MarshalAsAttribute(UnmanagedType.SysInt)]
			public int significance;
			[MarshalAsAttribute(UnmanagedType.LPStr)]
			public string languageTranslationStrings;
		};

		public struct HighlightDefinition {
			public string Id;
			public bool UserDefaultInterest;
			public HighlightType HighlightTags;
			public HighlightSignificance Significance;
			public TranslationEntry[] NameTranslationTable;
		};

		[StructLayout(LayoutKind.Sequential),Serializable]
		private struct OpenGroupParamsInternal {
			[MarshalAsAttribute(UnmanagedType.LPStr)]
			public string id;
			[MarshalAsAttribute(UnmanagedType.LPStr)]
			public string groupDescriptionTable;
		};

		public struct OpenGroupParams {
			public string Id;
			public TranslationEntry[] GroupDescriptionTable;
		};

		[StructLayout(LayoutKind.Sequential),Serializable]
		public struct CloseGroupParams {
			[MarshalAsAttribute(UnmanagedType.LPStr)]
			public string id;
			[MarshalAsAttribute(UnmanagedType.I1)]
			public bool destroyHighlights;
		};

		[StructLayout(LayoutKind.Sequential),Serializable]
		public struct ScreenshotHighlightParams {
			[MarshalAsAttribute(UnmanagedType.LPStr)]
			public string groupId;
			[MarshalAsAttribute(UnmanagedType.LPStr)]
			public string highlightId;
		};

		[StructLayout(LayoutKind.Sequential),Serializable]
		public struct VideoHighlightParams {
			[MarshalAsAttribute(UnmanagedType.LPStr)]
			public string groupId;
			[MarshalAsAttribute(UnmanagedType.LPStr)]
			public string highlightId;
			[MarshalAsAttribute(UnmanagedType.SysInt)]
			public int startDelta;
			[MarshalAsAttribute(UnmanagedType.SysInt)]
			public int endDelta;
		};

		[StructLayout(LayoutKind.Sequential),Serializable]
		private struct GroupViewInternal {
			[MarshalAsAttribute(UnmanagedType.LPStr)]
			public string groupId;
			[MarshalAsAttribute(UnmanagedType.SysInt)]
			public int tagFilter;
			[MarshalAsAttribute(UnmanagedType.SysInt)]
			public int significanceFilter;
		};

		public struct GroupView {
			public string GroupId;
			public HighlightType TagFilter;
			public HighlightSignificance SignificanceFilter;
		};

		public struct RequestPermissionsParams
		{			
			public HighlightScope ScopesFlags;			
		};

		[StructLayout(LayoutKind.Sequential),Serializable]
		private struct RequestPermissionsParamsInternal
		{
			[MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 1)]
			public int scopesFlags;			
		};


		#if (UNITY_64 || UNITY_EDITOR_64 || PLATFORM_ARCH_64)
		const string DLL64Name = "HighlightsPlugin64";
		#else
		#error x86 builds not supported
		#endif

		[DllImport(DLL64Name, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
		private static extern int Create([MarshalAs(UnmanagedType.LPStr)] string appName, int n, IntPtr scopes);

		[DllImport(DLL64Name, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
		private static extern bool Release();

		[DllImport(DLL64Name, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
		private static extern void RequestPermissionsAsync();

		[DllImport(DLL64Name, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
		public static extern void GetUILanguageAsync();

		[DllImport(DLL64Name, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
		public static extern void SetDefaultLocale([MarshalAs(UnmanagedType.LPStr)] string defaultLocale);

		[DllImport(DLL64Name, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
		private static extern void HighlightsConfigure(int n, IntPtr highlightDefinitions);

		[DllImport(DLL64Name, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
		public static extern void GetUserSettings();

		[DllImport(DLL64Name, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
		private static extern void Highlights_OpenGroupAsync(IntPtr openGroupParams);

		[DllImport(DLL64Name, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
		private static extern void Highlights_CloseGroupAsync(IntPtr closeGroupParams);

		[DllImport(DLL64Name, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
		private static extern void Highlights_SetScreenshotHighlightAsync(IntPtr screenshotHighlightParams);
		
		[DllImport(DLL64Name, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
		private static extern void Highlights_SetVideoHighlightAsync(IntPtr videoHighlightParams);

		[DllImport(DLL64Name, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
		private static extern void Highlights_OpenSummaryAsync(int n, IntPtr summaryParams);

		[DllImport(DLL64Name, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
		private static extern void Highlights_GetNumberOfHighlightsAsync(IntPtr groupView);

		[DllImport(DLL64Name, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.BStr)]
		public static extern string GetInfoLog();

		[DllImport(DLL64Name, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.BStr)]
		public static extern string GetErrorLog();

		// Local functions
		//Constructs the main SDK interface. Also performs the version check.
		public static ReturnCode CreateHighlightsSDK(string AppName, HighlightScope[] RequiredScopes) {

			List<IntPtr> allocatedMemory = new List<IntPtr> ();

			IntPtr nativeArray = Marshal.AllocHGlobal (RequiredScopes.Length * Marshal.SizeOf (typeof(IntPtr)));

			for (int i = 0; i < RequiredScopes.Length; ++i) {
				IntPtr nativeScope = Marshal.AllocHGlobal (Marshal.SizeOf (typeof(Scope)));
				allocatedMemory.Add (nativeScope);
				Scope scope = new Scope();
				scope.value = (int)RequiredScopes [i];
				Marshal.StructureToPtr(scope, nativeScope, false);
				Marshal.WriteIntPtr (nativeArray, i * Marshal.SizeOf (typeof(IntPtr)), nativeScope);
			}

            ReturnCode ret = (ReturnCode)Create(AppName, RequiredScopes.Length, nativeArray);

            if (ret == ReturnCode.SUCCESS) {
				Debug.Log("Highlights SDK initialized successfully");
                instanceCreated = true;
			}
			else {
				Debug.LogError("Failed to initialize Highlights SDK");
			}

			Marshal.FreeHGlobal (nativeArray);

			foreach (IntPtr ptr in allocatedMemory) {
				Marshal.FreeHGlobal (ptr);
			}

            return ret;
		}

		//Release the main SDK interface after create.
		public static void ReleaseHighlightsSDK() {
			if (!instanceCreated) {
				Debug.LogError ("Highlights release failed as no running instance was found");
				return;
			}

			if (Release ()) 
				Debug.Log ("Highlights SDK released successfully");
			else 
				Debug.LogError ("Failed to release Highlights SDK");

			instanceCreated = false;
				
		}

		// Updates the unity log with info and error messages passed from the highlights sdk
		public static void UpdateLog() {
			string infoLog = GetInfoLog ();
			if (infoLog != "") {
				Debug.Log (infoLog);
			}

			string errorLog = GetErrorLog ();
			if (errorLog != "") {
				Debug.LogError (errorLog);
			}
		}

		// Configure Highlights. Takes an array of highlight definition objects to define highlights in the game
		public static void ConfigureHighlights(HighlightDefinition[] highlightDefinitions) {
			if (!instanceCreated) {
				Debug.LogError ("ERROR: Cannot configure Highlights. The SDK has not been initialized.");
				return;
			}				

			var allocatedMemory = new List<IntPtr> ();

			IntPtr nativeArray = Marshal.AllocHGlobal (Marshal.SizeOf (typeof(IntPtr)) * highlightDefinitions.Length);

			try {
				
				for (int i = 0; i < highlightDefinitions.Length; ++i) {
					IntPtr nativeHighlightsDefinition = Marshal.AllocHGlobal (Marshal.SizeOf (typeof(HighlightDefinitionInternal)));
					HighlightDefinitionInternal hd = new HighlightDefinitionInternal();
					hd.id = highlightDefinitions [i].Id;
					hd.highlightTags = (int)(highlightDefinitions [i]).HighlightTags;
					hd.significance = (int)(highlightDefinitions [i]).Significance;
					hd.userDefaultInterest = (highlightDefinitions [i]).UserDefaultInterest;
					StringBuilder sb = new StringBuilder();
					foreach (TranslationEntry te in (highlightDefinitions [i]).NameTranslationTable) {
						sb.Append(te.Language).Append("\a").Append(te.Translation).Append("\a");
					}
					hd.languageTranslationStrings = sb.ToString ();
					allocatedMemory.Add (nativeHighlightsDefinition);
					Marshal.StructureToPtr (hd, nativeHighlightsDefinition, false);
					Marshal.WriteIntPtr (nativeArray, i * Marshal.SizeOf (typeof(IntPtr)), nativeHighlightsDefinition);
				}

				HighlightsConfigure (highlightDefinitions.Length, nativeArray);
			}
			finally {
				Marshal.FreeHGlobal (nativeArray);

				foreach (IntPtr ptr in allocatedMemory) {
					Marshal.FreeHGlobal (ptr);
				}
			}
		}

		// Request permissions from the user (if not already granted to provide permissions for highlight capture)
		public static void RequestPermissions() {			
			if (!instanceCreated) {
				Debug.LogError ("ERROR: Cannot request permissions. The SDK has not been initialized.");
				return;
			}
			
			RequestPermissionsAsync();
		}

		// Begins a "group" which groups several Highlights together.
		public static void OpenGroup(OpenGroupParams openGroupParams)
		{
			if (!instanceCreated) {
				Debug.LogError ("ERROR: Cannot open a group. The SDK has not been initialized.");
				return;
			}

			OpenGroupParamsInternal ogp = new OpenGroupParamsInternal();
			ogp.id = openGroupParams.Id;

			StringBuilder sb = new StringBuilder();
			foreach (TranslationEntry te in openGroupParams.GroupDescriptionTable) {
				sb.Append(te.Language).Append("\a").Append(te.Translation).Append("\a");
			}

			ogp.groupDescriptionTable = sb.ToString ();

			IntPtr pnt = Marshal.AllocHGlobal (Marshal.SizeOf (ogp));

			try
			{
				Marshal.StructureToPtr(ogp, pnt, false);
				Highlights_OpenGroupAsync (pnt);
			}
			finally {
				Marshal.FreeHGlobal (pnt);
			}

		}

		// Closes out a group and purges the unsaved contents.
		public static void CloseGroup(CloseGroupParams closeGroupParams)
		{
			if (!instanceCreated) {
				Debug.LogError ("ERROR: Cannot close a group. The SDK has not been initialized.");
				return;
			}

			IntPtr pnt = Marshal.AllocHGlobal (Marshal.SizeOf (closeGroupParams));

			try
			{
				Marshal.StructureToPtr(closeGroupParams, pnt, false);
				Highlights_CloseGroupAsync (pnt);
			}
			finally {
				Marshal.FreeHGlobal (pnt);
			}
		}

		// Records a screenshot highlight for the given group. Attached metadata to it to make the Highlight more interesting.
		public static void SetScreenshotHighlight(ScreenshotHighlightParams screenshotHighlightParams) {
			if (!instanceCreated) {
				Debug.LogError ("ERROR: Cannot take a screenshot. The SDK has not been initialized.");
				return;
			}

			IntPtr pnt = Marshal.AllocHGlobal (Marshal.SizeOf (screenshotHighlightParams));

			try
			{
				Marshal.StructureToPtr(screenshotHighlightParams, pnt, false);
				Highlights_SetScreenshotHighlightAsync (pnt);
			}
			finally {
				Marshal.FreeHGlobal (pnt);
			}
		}

		// Records a video highlight for the given group. Attached metadata to it to make the Highlight more interesting.
		// Set the start and end delta to change the length of the video clip.
		public static void SetVideoHighlight(VideoHighlightParams videoHighlightParams) {
			if (!instanceCreated) {
				Debug.LogError ("ERROR: Cannot record a video. The SDK has not been initialized.");
				return;
			}

			IntPtr pnt = Marshal.AllocHGlobal (Marshal.SizeOf (videoHighlightParams));

			try
			{
				Marshal.StructureToPtr(videoHighlightParams, pnt, false);
				Highlights_SetVideoHighlightAsync (pnt);
			}
			finally {
				Marshal.FreeHGlobal (pnt);
			}
		}

		// Opens up Summary Dialog for one or more groups
		public static void OpenSummary(GroupView[] summaryParams) {
			if (!instanceCreated) {
				Debug.LogError ("ERROR: Cannot open summary. The SDK has not been initialized.");
				return;
			}
				
			List<IntPtr> allocatedMemory = new List<IntPtr> ();
			IntPtr nativeArray = Marshal.AllocHGlobal (Marshal.SizeOf (typeof(IntPtr)) * summaryParams.Length);

			try {

				for (int i = 0; i < summaryParams.Length; ++i) {
					GroupViewInternal gvi = new GroupViewInternal ();
					gvi.groupId = summaryParams[i].GroupId;
					gvi.significanceFilter = (int)(summaryParams[i]).SignificanceFilter;
					gvi.tagFilter = (int)(summaryParams[i]).TagFilter;

					IntPtr nativeSummaryParams = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(GroupViewInternal)));
					allocatedMemory.Add (nativeSummaryParams);
					Marshal.StructureToPtr (gvi, nativeSummaryParams, false);
					Marshal.WriteIntPtr (nativeArray, Marshal.SizeOf (typeof(IntPtr)) * i, nativeSummaryParams);
				}

				Highlights_OpenSummaryAsync (summaryParams.Length, nativeArray);
			}
			finally {
				Marshal.FreeHGlobal (nativeArray);

				foreach (IntPtr ptr in allocatedMemory) {
					Marshal.FreeHGlobal (ptr);
				}
			}
		}

		// Retrieves the number of highlights given the group ID and filtering params
		public static void GetNumberOfHighlights(GroupView groupView) {
			if (!instanceCreated) {
				Debug.LogError ("ERROR: Cannot get number of highlights. The SDK has not been initialized.");
				return;
			}

			GroupViewInternal spi = new GroupViewInternal ();
			spi.groupId = groupView.GroupId;
			spi.significanceFilter = (int)groupView.SignificanceFilter;
			spi.tagFilter = (int)groupView.TagFilter;

			IntPtr pnt = Marshal.AllocHGlobal (Marshal.SizeOf (spi));

			try	{
				Marshal.StructureToPtr(spi, pnt, false);
				Highlights_GetNumberOfHighlightsAsync (pnt);
			}
			finally {
				Marshal.FreeHGlobal (pnt);
			}
		}
	}
}