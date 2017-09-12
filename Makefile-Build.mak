#
#   eduVPN - End-user friendly VPN
#
#   Copyright: 2017, The Commons Conservancy eduVPN Programme
#   SPDX-License-Identifier: GPL-3.0+
#

SETUP_NAME=eduVPN-Client-Win-$(PLAT)
!IF "$(CFG)" == "Debug"
SETUP_NAME=$(SETUP_NAME)D
!ENDIF

# WiX parameters
!IF "$(PLAT)" == "x64"
WIX_PROGRAM_FILES_FOLDER=ProgramFiles64Folder
!ELSE
WIX_PROGRAM_FILES_FOLDER=ProgramFilesFolder
!ENDIF
WIX_WIXCOP_FLAGS=-nologo "-set1$(MAKEDIR)\wixcop.xml"
WIX_CANDLE_FLAGS=-nologo -arch $(PLAT) "-deduVPN.TargetDir=bin\$(CFG)\$(PLAT)\\" -deduVPN.ProgramFilesFolder=$(WIX_PROGRAM_FILES_FOLDER) -ext WixNetFxExtension
WIX_LIGHT_FLAGS=-nologo -dcl:high -spdb -sice:ICE03 -sice:ICE82 -ext WixNetFxExtension

!IF "$(CFG)" == "Debug"
VC150REDIST_MSM=Microsoft_VC150_DebugCRT_$(PLAT).msm
!ELSE
VC150REDIST_MSM=Microsoft_VC150_CRT_$(PLAT).msm
!ENDIF


Clean ::
	-devenv.com "eduVPN.sln" /Clean "$(CFG)|$(PLAT)" $(DEVENV_FLAGS)
	-if exist "$(OUTPUT_DIR)\$(CFG)\$(PLAT)\eduVPN.wixobj"            del /f /q "$(OUTPUT_DIR)\$(CFG)\$(PLAT)\eduVPN.wixobj"
	-if exist "$(OUTPUT_DIR)\$(CFG)\$(PLAT)\eduOpenVPN.dll.wixobj"    del /f /q "$(OUTPUT_DIR)\$(CFG)\$(PLAT)\eduOpenVPN.dll.wixobj"
	-if exist "$(OUTPUT_DIR)\$(CFG)\$(PLAT)\eduVPN.dll.wixobj"        del /f /q "$(OUTPUT_DIR)\$(CFG)\$(PLAT)\eduVPN.dll.wixobj"
	-if exist "$(OUTPUT_DIR)\$(CFG)\$(PLAT)\eduVPN.Client.exe.wixobj" del /f /q "$(OUTPUT_DIR)\$(CFG)\$(PLAT)\eduVPN.Client.exe.wixobj"
	-if exist "$(SETUP_DIR)\$(SETUP_NAME).msi"                        del /f /q "$(SETUP_DIR)\$(SETUP_NAME).msi"


######################################################################
# Setup
######################################################################

!IF "$(CFG)" == "Release"
Setup :: \
	SetupBuild \
	"$(SETUP_DIR)\$(SETUP_NAME).msi"

SetupBuild ::
	devenv.com "eduVPN.sln" /Build "$(CFG)|$(PLAT)" $(DEVENV_FLAGS)
!ENDIF


######################################################################
# Building
######################################################################

"$(OUTPUT_DIR)\$(CFG)\$(PLAT)\eduVPN.Client.exe" ::
	devenv.com "eduVPN.sln" /Build "$(CFG)|$(PLAT)" $(DEVENV_FLAGS)

"$(OUTPUT_DIR)\$(CFG)\$(PLAT)\$(VC150REDIST_MSM)" : "$(VCINSTALLDIR)Redist\MSVC\14.10.25008\MergeModules\$(VC150REDIST_MSM)"
	copy /y $** $@ > NUL

"$(OUTPUT_DIR)\$(CFG)\$(PLAT)\VC150Redist.wixobj" : "$(OUTPUT_DIR)\$(CFG)\$(PLAT)\$(VC150REDIST_MSM)"
	"$(WIX)bin\candle.exe" $(WIX_CANDLE_FLAGS) -out $@ <<
<?xml version="1.0" encoding="utf-8"?>
<Wix xmlns="http://schemas.microsoft.com/wix/2006/wi">
    <Fragment>
        <DirectoryRef Id="TARGETDIR"><Merge Id="VC150Redist" SourceFile="$(VC150REDIST_MSM)" DiskId="1" Language="0"/></DirectoryRef>
    </Fragment>
</Wix>
<<NOKEEP

"$(OUTPUT_DIR)\$(CFG)\$(PLAT)\eduVPN.wixobj" : "eduVPN.wxs"
	"$(WIX)bin\wixcop.exe" $(WIX_WIXCOP_FLAGS) $**
	"$(WIX)bin\candle.exe" $(WIX_CANDLE_FLAGS) -out $@ $**

"$(OUTPUT_DIR)\$(CFG)\$(PLAT)\eduEd25519.dll.wixobj" : "eduEd25519\eduEd25519\eduEd25519.wxs"
	"$(WIX)bin\wixcop.exe" $(WIX_WIXCOP_FLAGS) $**
	"$(WIX)bin\candle.exe" $(WIX_CANDLE_FLAGS) -out $@ $**

"$(OUTPUT_DIR)\$(CFG)\$(PLAT)\eduJSON.dll.wixobj" : "eduJSON\eduJSON\eduJSON.wxs"
	"$(WIX)bin\wixcop.exe" $(WIX_WIXCOP_FLAGS) $**
	"$(WIX)bin\candle.exe" $(WIX_CANDLE_FLAGS) -out $@ $**

"$(OUTPUT_DIR)\$(CFG)\$(PLAT)\eduOAuth.dll.wixobj" : "eduOAuth\eduOAuth\eduOAuth.wxs"
	"$(WIX)bin\wixcop.exe" $(WIX_WIXCOP_FLAGS) $**
	"$(WIX)bin\candle.exe" $(WIX_CANDLE_FLAGS) -out $@ $**

"$(OUTPUT_DIR)\$(CFG)\$(PLAT)\eduOpenVPN.dll.wixobj" : "eduOpenVPN\eduOpenVPN\eduOpenVPN.wxs"
	"$(WIX)bin\wixcop.exe" $(WIX_WIXCOP_FLAGS) $**
	"$(WIX)bin\candle.exe" $(WIX_CANDLE_FLAGS) -out $@ $**

"$(OUTPUT_DIR)\$(CFG)\$(PLAT)\eduVPN.dll.wixobj" : "eduVPN\eduVPN.wxs"
	"$(WIX)bin\wixcop.exe" $(WIX_WIXCOP_FLAGS) $**
	"$(WIX)bin\candle.exe" $(WIX_CANDLE_FLAGS) -out $@ $**

"$(OUTPUT_DIR)\$(CFG)\$(PLAT)\eduVPN.Client.exe.wixobj" : "eduVPN.Client\eduVPN.Client.wxs"
	"$(WIX)bin\wixcop.exe" $(WIX_WIXCOP_FLAGS) $**
	"$(WIX)bin\candle.exe" $(WIX_CANDLE_FLAGS) -out $@ $**

"$(SETUP_DIR)\$(SETUP_NAME).msi" : \
	"$(OUTPUT_DIR)\$(CFG)\$(PLAT)\$(SETUP_NAME).sl.mst" \
	"$(OUTPUT_DIR)\$(CFG)\$(PLAT)\$(SETUP_NAME).msi"
	-if exist $@ del /f /q $@
	-if exist "$(@:"=).tmp" del /f /q "$(@:"=).tmp"
	copy /y "$(OUTPUT_DIR)\$(CFG)\$(PLAT)\$(SETUP_NAME).msi" "$(@:"=).tmp" > NUL
	cscript.exe $(CSCRIPT_FLAGS) "bin\MSI.wsf" //Job:AddStorage "$(@:"=).tmp" "$(OUTPUT_DIR)\$(CFG)\$(PLAT)\$(SETUP_NAME).sl.mst" 1060 /L
!IFDEF MANIFESTCERTIFICATETHUMBPRINT
	signtool.exe sign /sha1 "$(MANIFESTCERTIFICATETHUMBPRINT)" /t "$(MANIFESTTIMESTAMPURL)" /d "eduVPN Client for Windows" /q "$(@:"=).tmp"
!ENDIF
	move /y "$(@:"=).tmp" $@ > NUL


######################################################################
# Locale-specific rules
######################################################################

LANG=en-US
!INCLUDE "Makefile-BuildLocal.mak"

LANG=sl
!INCLUDE "Makefile-BuildLocal.mak"
