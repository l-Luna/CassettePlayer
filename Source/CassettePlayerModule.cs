using System;
using System.Runtime.InteropServices;
using FMOD;
using INITFLAGS = FMOD.Studio.INITFLAGS;

namespace Celeste.Mod.CassettePlayer;

public class CassettePlayerModule : EverestModule{
	public static CassettePlayerModule Instance{ get; private set; }

	public CassettePlayerModule(){
		Instance = this;
#if DEBUG
		// debug builds use verbose logging
		Logger.SetLogLevel(nameof(CassettePlayerModule), LogLevel.Verbose);
#else
        // release builds use info logging to reduce spam in log files
        Logger.SetLogLevel(nameof(CassettePlayerModule), LogLevel.Info);
#endif
	}

	public static void LogInfo(string s){
		Logger.Log(LogLevel.Info, "CassettePlayer", s);
	}

	public override void Load(){
		On.FMOD.Studio.System.initialize += FmodStudioSystemInit;
	}

	public override void Unload(){
		On.FMOD.Studio.System.initialize -= FmodStudioSystemInit;
	}

	private static RESULT FmodStudioSystemInit(
		On.FMOD.Studio.System.orig_initialize orig,
		FMOD.Studio.System self,
		int maxchannels,
		INITFLAGS studioFlags,
		FMOD.INITFLAGS flags,
		IntPtr extradriverdata){
		var result = orig(self, maxchannels, studioFlags, flags, extradriverdata);

		if (result == RESULT.OK){
			Audio.CheckFmod(self.getLowLevelSystem(out var llSys));

			string arch = RuntimeInformation.OSArchitecture is Architecture.X86 or Architecture.X64
				? "x86"
				: "arm";
			string osName =
				RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "cassette_player.dll" :
				RuntimeInformation.IsOSPlatform(OSPlatform.Linux) ? "libcassette_player.so" :
				$"libcassette_player.{arch}.dylib";

			string hardPath = Everest.Content.Get($"Libs/{osName}").GetCachedPath();
			Audio.CheckFmod(llSys.loadPlugin(hardPath, out _));
			LogInfo("Loaded cassette player FMOD DSP plugin");
		}

		return result;
	}
}