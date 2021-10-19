// ◇ UTF-8

//**********************************
// THIS FILE IS AUTO GENERATED FILE.
// DON'T EDIT THIS FILE.
// EDIT TEMPLATE FILE INSTEAD.
//**********************************
  
var EmotePlayerJS = {

	//----------------------------------------------------------------------
	Native_Emote_GetSDKVersion : function() {
		// console.log("Emote_GetSDKVersion:");
		return ccall("EMS_Emote_GetSDKVersion", "number", [], []);
	},
	Native_Emote_GetSDKVersion__deps: ['EMS_Emote_GetSDKVersion'],

	//----------------------------------------------------------------------
	Native_Emote_GetBuildDateTime : function() {
		// console.log("Emote_GetBuildDateTime:");
		return ccall("EMS_Emote_GetBuildDateTime", "number", [], []);
	},
	Native_Emote_GetBuildDateTime__deps: ['EMS_Emote_GetBuildDateTime'],

	//----------------------------------------------------------------------
	Native_EmoteDevice_SetInitializeParams : function(mainMemSize) {
		// console.log("EmoteDevice_SetInitializeParams:mainMemSize=%d", mainMemSize);
		return ccall("EMS_EmoteDevice_SetInitializeParams", "number", ["number"], [mainMemSize]);
	},
	Native_EmoteDevice_SetInitializeParams__deps: ['EMS_EmoteDevice_SetInitializeParams'],

	//----------------------------------------------------------------------
	Native_EmoteDevice_GetMainMemUsage : function() {
		// console.log("EmoteDevice_GetMainMemUsage:");
		return ccall("EMS_EmoteDevice_GetMainMemUsage", "number", [], []);
	},
	Native_EmoteDevice_GetMainMemUsage__deps: ['EMS_EmoteDevice_GetMainMemUsage'],

	//----------------------------------------------------------------------
	Native_EmoteDevice_GetMainMemTotalSize : function() {
		// console.log("EmoteDevice_GetMainMemTotalSize:");
		return ccall("EMS_EmoteDevice_GetMainMemTotalSize", "number", [], []);
	},
	Native_EmoteDevice_GetMainMemTotalSize__deps: ['EMS_EmoteDevice_GetMainMemTotalSize'],

	//----------------------------------------------------------------------
	Native_EmoteDevice_GetMainMemAllocatedSize : function() {
		// console.log("EmoteDevice_GetMainMemAllocatedSize:");
		return ccall("EMS_EmoteDevice_GetMainMemAllocatedSize", "number", [], []);
	},
	Native_EmoteDevice_GetMainMemAllocatedSize__deps: ['EMS_EmoteDevice_GetMainMemAllocatedSize'],

	//----------------------------------------------------------------------
	Native_Emote_CheckValidObject : function(imagePtr, imageSize) {
		// console.log("Emote_CheckValidObject:imagePtr=%d, imageSize=%d", imagePtr, imageSize);
		return ccall("EMS_Emote_CheckValidObject", "number", ["number", "number"], [imagePtr, imageSize]);
	},
	Native_Emote_CheckValidObject__deps: ['EMS_Emote_CheckValidObject'],

	//----------------------------------------------------------------------
	Native_EmoteDevice_Require : function() {
		// console.log("EmoteDevice_Require:");
		return ccall("EMS_EmoteDevice_Require", "number", [], []);
	},
	Native_EmoteDevice_Require__deps: ['EMS_EmoteDevice_Require'],

	//----------------------------------------------------------------------
	Native_EmoteDevice_Release : function() {
		// console.log("EmoteDevice_Release:");
		return ccall("EMS_EmoteDevice_Release", "number", [], []);
	},
	Native_EmoteDevice_Release__deps: ['EMS_EmoteDevice_Release'],

	//----------------------------------------------------------------------
	Native_EmoteDevice_RefCount : function() {
		// console.log("EmoteDevice_RefCount:");
		return ccall("EMS_EmoteDevice_RefCount", "number", [], []);
	},
	Native_EmoteDevice_RefCount__deps: ['EMS_EmoteDevice_RefCount'],

	//----------------------------------------------------------------------
	Native_EmoteDevice_CountPlayer : function() {
		// console.log("EmoteDevice_CountPlayer:");
		return ccall("EMS_EmoteDevice_CountPlayer", "number", [], []);
	},
	Native_EmoteDevice_CountPlayer__deps: ['EMS_EmoteDevice_CountPlayer'],

	//----------------------------------------------------------------------
	Native_EmoteDevice_Initialize : function() {
		// console.log("EmoteDevice_Initialize:");
		return ccall("EMS_EmoteDevice_Initialize", "number", [], []);
	},
	Native_EmoteDevice_Initialize__deps: ['EMS_EmoteDevice_Initialize'],

	//----------------------------------------------------------------------
	Native_EmoteDevice_Finish : function() {
		// console.log("EmoteDevice_Finish:");
		return ccall("EMS_EmoteDevice_Finish", "number", [], []);
	},
	Native_EmoteDevice_Finish__deps: ['EMS_EmoteDevice_Finish'],

	//----------------------------------------------------------------------
	Native_EmoteDevice_SetMaskRegionClipping : function(value) {
		// console.log("EmoteDevice_SetMaskRegionClipping:value=%d", value);
		return ccall("EMS_EmoteDevice_SetMaskRegionClipping", "number", ["number"], [value]);
	},
	Native_EmoteDevice_SetMaskRegionClipping__deps: ['EMS_EmoteDevice_SetMaskRegionClipping'],

	//----------------------------------------------------------------------
	Native_EmoteDevice_SetProtectTranslucentTextureColor : function(value) {
		// console.log("EmoteDevice_SetProtectTranslucentTextureColor:value=%d", value);
		return ccall("EMS_EmoteDevice_SetProtectTranslucentTextureColor", "number", ["number"], [value]);
	},
	Native_EmoteDevice_SetProtectTranslucentTextureColor__deps: ['EMS_EmoteDevice_SetProtectTranslucentTextureColor'],

	//----------------------------------------------------------------------
	Native_EmoteDevice_SetForceBufferedBlend : function(value) {
		// console.log("EmoteDevice_SetForceBufferedBlend:value=%d", value);
		return ccall("EMS_EmoteDevice_SetForceBufferedBlend", "number", ["number"], [value]);
	},
	Native_EmoteDevice_SetForceBufferedBlend__deps: ['EMS_EmoteDevice_SetForceBufferedBlend'],

	//----------------------------------------------------------------------
	Native_EmoteDevice_SetGpuMeshDeformationEnabled : function(value) {
		// console.log("EmoteDevice_SetGpuMeshDeformationEnabled:value=%d", value);
		return ccall("EMS_EmoteDevice_SetGpuMeshDeformationEnabled", "number", ["number"], [value]);
	},
	Native_EmoteDevice_SetGpuMeshDeformationEnabled__deps: ['EMS_EmoteDevice_SetGpuMeshDeformationEnabled'],

	//----------------------------------------------------------------------
	Native_EmotePlayer_Initialize : function(psbRefArray, arraySize) {
		// console.log("EmotePlayer_Initialize:psbRefArray=%d, arraySize=%d", psbRefArray, arraySize);
		return ccall("EMS_EmotePlayer_Initialize", "number", ["number", "number"], [psbRefArray, arraySize]);
	},
	Native_EmotePlayer_Initialize__deps: ['EMS_EmotePlayer_Initialize'],

	//----------------------------------------------------------------------
	Native_EmotePlayer_Finish : function(playerId) {
		// console.log("EmotePlayer_Finish:playerId=%d", playerId);
		return ccall("EMS_EmotePlayer_Finish", "number", ["number"], [playerId]);
	},
	Native_EmotePlayer_Finish__deps: ['EMS_EmotePlayer_Finish'],

	//----------------------------------------------------------------------
	Native_EmotePlayer_SetScreenSize : function(playerId, width, height) {
		// console.log("EmotePlayer_SetScreenSize:playerId=%d, width=%d, height=%d", playerId, width, height);
		return ccall("EMS_EmotePlayer_SetScreenSize", "number", ["number", "number", "number"], [playerId, width, height]);
	},
	Native_EmotePlayer_SetScreenSize__deps: ['EMS_EmotePlayer_SetScreenSize'],

	//----------------------------------------------------------------------
	Native_EmotePlayer_SetPixelScale : function(playerId, x, y) {
		// console.log("EmotePlayer_SetPixelScale:playerId=%d, x=%f, y=%f", playerId, x, y);
		return ccall("EMS_EmotePlayer_SetPixelScale", "number", ["number", "number", "number"], [playerId, x, y]);
	},
	Native_EmotePlayer_SetPixelScale__deps: ['EMS_EmotePlayer_SetPixelScale'],

	//----------------------------------------------------------------------
	Native_EmotePlayer_SetCommandBufferOutputPrimitive : function(playerId, primitive) {
		// console.log("EmotePlayer_SetCommandBufferOutputPrimitive:playerId=%d, primitive=%d", playerId, primitive);
		return ccall("EMS_EmotePlayer_SetCommandBufferOutputPrimitive", "number", ["number", "number"], [playerId, primitive]);
	},
	Native_EmotePlayer_SetCommandBufferOutputPrimitive__deps: ['EMS_EmotePlayer_SetCommandBufferOutputPrimitive'],

	//----------------------------------------------------------------------
	Native_EmotePlayer_Update : function(playerId, multiplier) {
		// console.log("EmotePlayer_Update:playerId=%d, multiplier=%f", playerId, multiplier);
		return ccall("EMS_EmotePlayer_Update", "number", ["number", "number"], [playerId, multiplier]);
	},
	Native_EmotePlayer_Update__deps: ['EMS_EmotePlayer_Update'],

	//----------------------------------------------------------------------
	Native_EmotePlayer_Draw : function(playerId) {
		// console.log("EmotePlayer_Draw:playerId=%d", playerId);
		return ccall("EMS_EmotePlayer_Draw", "number", ["number"], [playerId]);
	},
	Native_EmotePlayer_Draw__deps: ['EMS_EmotePlayer_Draw'],

	//----------------------------------------------------------------------
	Native_EmotePlayer_GetCommandBufferLength : function(playerId) {
		// console.log("EmotePlayer_GetCommandBufferLength:playerId=%d", playerId);
		return ccall("EMS_EmotePlayer_GetCommandBufferLength", "number", ["number"], [playerId]);
	},
	Native_EmotePlayer_GetCommandBufferLength__deps: ['EMS_EmotePlayer_GetCommandBufferLength'],

	//----------------------------------------------------------------------
	Native_EmotePlayer_GetCommandBuffer : function(playerId) {
		// console.log("EmotePlayer_GetCommandBuffer:playerId=%d", playerId);
		return ccall("EMS_EmotePlayer_GetCommandBuffer", "number", ["number"], [playerId]);
	},
	Native_EmotePlayer_GetCommandBuffer__deps: ['EMS_EmotePlayer_GetCommandBuffer'],

	//----------------------------------------------------------------------
	Native_EmotePlayer_GetCommandBufferMaxImageCount : function(playerId) {
		// console.log("EmotePlayer_GetCommandBufferMaxImageCount:playerId=%d", playerId);
		return ccall("EMS_EmotePlayer_GetCommandBufferMaxImageCount", "number", ["number"], [playerId]);
	},
	Native_EmotePlayer_GetCommandBufferMaxImageCount__deps: ['EMS_EmotePlayer_GetCommandBufferMaxImageCount'],

	//----------------------------------------------------------------------
	Native_EmotePlayer_GetCommandBufferBuiltinImageCount : function(playerId) {
		// console.log("EmotePlayer_GetCommandBufferBuiltinImageCount:playerId=%d", playerId);
		return ccall("EMS_EmotePlayer_GetCommandBufferBuiltinImageCount", "number", ["number"], [playerId]);
	},
	Native_EmotePlayer_GetCommandBufferBuiltinImageCount__deps: ['EMS_EmotePlayer_GetCommandBufferBuiltinImageCount'],

	//----------------------------------------------------------------------
	Native_EmotePlayer_GetCommandBufferBuiltinImageInfo : function(playerId, index, imageInfo) {
		// console.log("EmotePlayer_GetCommandBufferBuiltinImageInfo:playerId=%d, index=%d, imageInfo=%d", playerId, index, imageInfo);
		return ccall("EMS_EmotePlayer_GetCommandBufferBuiltinImageInfo", "number", ["number", "number", "number"], [playerId, index, imageInfo]);
	},
	Native_EmotePlayer_GetCommandBufferBuiltinImageInfo__deps: ['EMS_EmotePlayer_GetCommandBufferBuiltinImageInfo'],

	//----------------------------------------------------------------------
	Native_EmotePlayer_ExtractCommandBufferBuiltinImage : function(playerId, index, imagePtr, format) {
		// console.log("EmotePlayer_ExtractCommandBufferBuiltinImage:playerId=%d, index=%d, imagePtr=%d, format=%d", playerId, index, imagePtr, format);
		return ccall("EMS_EmotePlayer_ExtractCommandBufferBuiltinImage", "number", ["number", "number", "number", "number"], [playerId, index, imagePtr, format]);
	},
	Native_EmotePlayer_ExtractCommandBufferBuiltinImage__deps: ['EMS_EmotePlayer_ExtractCommandBufferBuiltinImage'],

	//----------------------------------------------------------------------
	Native_EmotePlayer_DestroyCommandBufferBuiltinImage : function(playerId, index) {
		// console.log("EmotePlayer_DestroyCommandBufferBuiltinImage:playerId=%d, index=%d", playerId, index);
		return ccall("EMS_EmotePlayer_DestroyCommandBufferBuiltinImage", "number", ["number", "number"], [playerId, index]);
	},
	Native_EmotePlayer_DestroyCommandBufferBuiltinImage__deps: ['EMS_EmotePlayer_DestroyCommandBufferBuiltinImage'],

	//----------------------------------------------------------------------
	Native_EmotePlayer_SetColor : function(playerId, r, g, b, a, frameCount, easing) {
		// console.log("EmotePlayer_SetColor:playerId=%d, r=%f, g=%f, b=%f, a=%f, frameCount=%f, easing=%f", playerId, r, g, b, a, frameCount, easing);
		return ccall("EMS_EmotePlayer_SetColor", "number", ["number", "number", "number", "number", "number", "number", "number"], [playerId, r, g, b, a, frameCount, easing]);
	},
	Native_EmotePlayer_SetColor__deps: ['EMS_EmotePlayer_SetColor'],

	//----------------------------------------------------------------------
	Native_EmotePlayer_Skip : function(playerId) {
		// console.log("EmotePlayer_Skip:playerId=%d", playerId);
		return ccall("EMS_EmotePlayer_Skip", "number", ["number"], [playerId]);
	},
	Native_EmotePlayer_Skip__deps: ['EMS_EmotePlayer_Skip'],

	//----------------------------------------------------------------------
	Native_EmotePlayer_Pass : function(playerId) {
		// console.log("EmotePlayer_Pass:playerId=%d", playerId);
		return ccall("EMS_EmotePlayer_Pass", "number", ["number"], [playerId]);
	},
	Native_EmotePlayer_Pass__deps: ['EMS_EmotePlayer_Pass'],

	//----------------------------------------------------------------------
	Native_EmotePlayer_Step : function(playerId) {
		// console.log("EmotePlayer_Step:playerId=%d", playerId);
		return ccall("EMS_EmotePlayer_Step", "number", ["number"], [playerId]);
	},
	Native_EmotePlayer_Step__deps: ['EMS_EmotePlayer_Step'],

	//----------------------------------------------------------------------
	Native_EmotePlayer_GetState : function(playerId, label) {
		// console.log("EmotePlayer_GetState:playerId=%d, label=[%s]", playerId, label);
		return ccall("EMS_EmotePlayer_GetState", "number", ["number", "number"], [playerId, label]);
	},
	Native_EmotePlayer_GetState__deps: ['EMS_EmotePlayer_GetState'],

	//----------------------------------------------------------------------
	Native_EmotePlayer_SetTransformOrderMask : function(playerId, mask) {
		// console.log("EmotePlayer_SetTransformOrderMask:playerId=%d, mask=%d", playerId, mask);
		return ccall("EMS_EmotePlayer_SetTransformOrderMask", "number", ["number", "number"], [playerId, mask]);
	},
	Native_EmotePlayer_SetTransformOrderMask__deps: ['EMS_EmotePlayer_SetTransformOrderMask'],

	//----------------------------------------------------------------------
	Native_EmotePlayer_SetCoord : function(playerId, x, y, frameCount, easing) {
		// console.log("EmotePlayer_SetCoord:playerId=%d, x=%f, y=%f, frameCount=%f, easing=%f", playerId, x, y, frameCount, easing);
		return ccall("EMS_EmotePlayer_SetCoord", "number", ["number", "number", "number", "number", "number"], [playerId, x, y, frameCount, easing]);
	},
	Native_EmotePlayer_SetCoord__deps: ['EMS_EmotePlayer_SetCoord'],

	//----------------------------------------------------------------------
	Native_EmotePlayer_SetScale : function(playerId, scale, frameCount, easing) {
		// console.log("EmotePlayer_SetScale:playerId=%d, scale=%f, frameCount=%f, easing=%f", playerId, scale, frameCount, easing);
		return ccall("EMS_EmotePlayer_SetScale", "number", ["number", "number", "number", "number"], [playerId, scale, frameCount, easing]);
	},
	Native_EmotePlayer_SetScale__deps: ['EMS_EmotePlayer_SetScale'],

	//----------------------------------------------------------------------
	Native_EmotePlayer_SetRot : function(playerId, rot, frameCount, easing) {
		// console.log("EmotePlayer_SetRot:playerId=%d, rot=%f, frameCount=%f, easing=%f", playerId, rot, frameCount, easing);
		return ccall("EMS_EmotePlayer_SetRot", "number", ["number", "number", "number", "number"], [playerId, rot, frameCount, easing]);
	},
	Native_EmotePlayer_SetRot__deps: ['EMS_EmotePlayer_SetRot'],

	//----------------------------------------------------------------------
	Native_EmotePlayer_SetGrayscale : function(playerId, rate, frameCount, easing) {
		// console.log("EmotePlayer_SetGrayscale:playerId=%d, rate=%f, frameCount=%f, easing=%f", playerId, rate, frameCount, easing);
		return ccall("EMS_EmotePlayer_SetGrayscale", "number", ["number", "number", "number", "number"], [playerId, rate, frameCount, easing]);
	},
	Native_EmotePlayer_SetGrayscale__deps: ['EMS_EmotePlayer_SetGrayscale'],

	//----------------------------------------------------------------------
	Native_EmotePlayer_SetAsOriginalScale : function(playerId, toggle) {
		// console.log("EmotePlayer_SetAsOriginalScale:playerId=%d, toggle=%d", playerId, toggle);
		return ccall("EMS_EmotePlayer_SetAsOriginalScale", "number", ["number", "number"], [playerId, toggle]);
	},
	Native_EmotePlayer_SetAsOriginalScale__deps: ['EMS_EmotePlayer_SetAsOriginalScale'],

	//----------------------------------------------------------------------
	Native_EmotePlayer_SetHairScale : function(playerId, scale) {
		// console.log("EmotePlayer_SetHairScale:playerId=%d, scale=%f", playerId, scale);
		return ccall("EMS_EmotePlayer_SetHairScale", "number", ["number", "number"], [playerId, scale]);
	},
	Native_EmotePlayer_SetHairScale__deps: ['EMS_EmotePlayer_SetHairScale'],

	//----------------------------------------------------------------------
	Native_EmotePlayer_SetPartsScale : function(playerId, scale) {
		// console.log("EmotePlayer_SetPartsScale:playerId=%d, scale=%f", playerId, scale);
		return ccall("EMS_EmotePlayer_SetPartsScale", "number", ["number", "number"], [playerId, scale]);
	},
	Native_EmotePlayer_SetPartsScale__deps: ['EMS_EmotePlayer_SetPartsScale'],

	//----------------------------------------------------------------------
	Native_EmotePlayer_SetBustScale : function(playerId, scale) {
		// console.log("EmotePlayer_SetBustScale:playerId=%d, scale=%f", playerId, scale);
		return ccall("EMS_EmotePlayer_SetBustScale", "number", ["number", "number"], [playerId, scale]);
	},
	Native_EmotePlayer_SetBustScale__deps: ['EMS_EmotePlayer_SetBustScale'],

	//----------------------------------------------------------------------
	Native_EmotePlayer_SetMeshDivisionRatio : function(playerId, ratio) {
		// console.log("EmotePlayer_SetMeshDivisionRatio:playerId=%d, ratio=%f", playerId, ratio);
		return ccall("EMS_EmotePlayer_SetMeshDivisionRatio", "number", ["number", "number"], [playerId, ratio]);
	},
	Native_EmotePlayer_SetMeshDivisionRatio__deps: ['EMS_EmotePlayer_SetMeshDivisionRatio'],

	//----------------------------------------------------------------------
	Native_EmotePlayer_CountMainTimelines : function(playerId) {
		// console.log("EmotePlayer_CountMainTimelines:playerId=%d", playerId);
		return ccall("EMS_EmotePlayer_CountMainTimelines", "number", ["number"], [playerId]);
	},
	Native_EmotePlayer_CountMainTimelines__deps: ['EMS_EmotePlayer_CountMainTimelines'],

	//----------------------------------------------------------------------
	Native_EmotePlayer_GetMainTimelineLabelAt : function(playerId, index) {
		// console.log("EmotePlayer_GetMainTimelineLabelAt:playerId=%d, index=%d", playerId, index);
		return ccall("EMS_EmotePlayer_GetMainTimelineLabelAt", "number", ["number", "number"], [playerId, index]);
	},
	Native_EmotePlayer_GetMainTimelineLabelAt__deps: ['EMS_EmotePlayer_GetMainTimelineLabelAt'],

	//----------------------------------------------------------------------
	Native_EmotePlayer_CountDiffTimelines : function(playerId) {
		// console.log("EmotePlayer_CountDiffTimelines:playerId=%d", playerId);
		return ccall("EMS_EmotePlayer_CountDiffTimelines", "number", ["number"], [playerId]);
	},
	Native_EmotePlayer_CountDiffTimelines__deps: ['EMS_EmotePlayer_CountDiffTimelines'],

	//----------------------------------------------------------------------
	Native_EmotePlayer_GetDiffTimelineLabelAt : function(playerId, index) {
		// console.log("EmotePlayer_GetDiffTimelineLabelAt:playerId=%d, index=%d", playerId, index);
		return ccall("EMS_EmotePlayer_GetDiffTimelineLabelAt", "number", ["number", "number"], [playerId, index]);
	},
	Native_EmotePlayer_GetDiffTimelineLabelAt__deps: ['EMS_EmotePlayer_GetDiffTimelineLabelAt'],

	//----------------------------------------------------------------------
	Native_EmotePlayer_IsLoopTimeline : function(playerId, label) {
		// console.log("EmotePlayer_IsLoopTimeline:playerId=%d, label=[%s]", playerId, label);
		return ccall("EMS_EmotePlayer_IsLoopTimeline", "number", ["number", "number"], [playerId, label]);
	},
	Native_EmotePlayer_IsLoopTimeline__deps: ['EMS_EmotePlayer_IsLoopTimeline'],

	//----------------------------------------------------------------------
	Native_EmotePlayer_GetTimelineTotalFrameCount : function(playerId, label) {
		// console.log("EmotePlayer_GetTimelineTotalFrameCount:playerId=%d, label=[%s]", playerId, label);
		return ccall("EMS_EmotePlayer_GetTimelineTotalFrameCount", "number", ["number", "number"], [playerId, label]);
	},
	Native_EmotePlayer_GetTimelineTotalFrameCount__deps: ['EMS_EmotePlayer_GetTimelineTotalFrameCount'],

	//----------------------------------------------------------------------
	Native_EmotePlayer_PlayTimeline : function(playerId, label, flags) {
		// console.log("EmotePlayer_PlayTimeline:playerId=%d, label=[%s], flags=%d", playerId, label, flags);
		return ccall("EMS_EmotePlayer_PlayTimeline", "number", ["number", "number", "number"], [playerId, label, flags]);
	},
	Native_EmotePlayer_PlayTimeline__deps: ['EMS_EmotePlayer_PlayTimeline'],

	//----------------------------------------------------------------------
	Native_EmotePlayer_StopTimeline : function(playerId, label) {
		// console.log("EmotePlayer_StopTimeline:playerId=%d, label=[%s]", playerId, label);
		return ccall("EMS_EmotePlayer_StopTimeline", "number", ["number", "number"], [playerId, label]);
	},
	Native_EmotePlayer_StopTimeline__deps: ['EMS_EmotePlayer_StopTimeline'],

	//----------------------------------------------------------------------
	Native_EmotePlayer_SetTimelineBlendRatio : function(playerId, label, value, frameCount, easing, stopWhenBlendDone) {
		// console.log("EmotePlayer_SetTimelineBlendRatio:playerId=%d, label=[%s], value=%f, frameCount=%f, easing=%f, stopWhenBlendDone=%d", playerId, label, value, frameCount, easing, stopWhenBlendDone);
		return ccall("EMS_EmotePlayer_SetTimelineBlendRatio", "number", ["number", "number", "number", "number", "number", "number"], [playerId, label, value, frameCount, easing, stopWhenBlendDone]);
	},
	Native_EmotePlayer_SetTimelineBlendRatio__deps: ['EMS_EmotePlayer_SetTimelineBlendRatio'],

	//----------------------------------------------------------------------
	Native_EmotePlayer_GetTimelineBlendRatio : function(playerId, label) {
		// console.log("EmotePlayer_GetTimelineBlendRatio:playerId=%d, label=[%s]", playerId, label);
		return ccall("EMS_EmotePlayer_GetTimelineBlendRatio", "number", ["number", "number"], [playerId, label]);
	},
	Native_EmotePlayer_GetTimelineBlendRatio__deps: ['EMS_EmotePlayer_GetTimelineBlendRatio'],

	//----------------------------------------------------------------------
	Native_EmotePlayer_FadeInTimeline : function(playerId, label, frameCount, easing) {
		// console.log("EmotePlayer_FadeInTimeline:playerId=%d, label=[%s], frameCount=%f, easing=%f", playerId, label, frameCount, easing);
		return ccall("EMS_EmotePlayer_FadeInTimeline", "number", ["number", "number", "number", "number"], [playerId, label, frameCount, easing]);
	},
	Native_EmotePlayer_FadeInTimeline__deps: ['EMS_EmotePlayer_FadeInTimeline'],

	//----------------------------------------------------------------------
	Native_EmotePlayer_FadeOutTimeline : function(playerId, label, frameCount, easing) {
		// console.log("EmotePlayer_FadeOutTimeline:playerId=%d, label=[%s], frameCount=%f, easing=%f", playerId, label, frameCount, easing);
		return ccall("EMS_EmotePlayer_FadeOutTimeline", "number", ["number", "number", "number", "number"], [playerId, label, frameCount, easing]);
	},
	Native_EmotePlayer_FadeOutTimeline__deps: ['EMS_EmotePlayer_FadeOutTimeline'],

	//----------------------------------------------------------------------
	Native_EmotePlayer_IsTimelinePlaying : function(playerId, label) {
		// console.log("EmotePlayer_IsTimelinePlaying:playerId=%d, label=[%s]", playerId, label);
		return ccall("EMS_EmotePlayer_IsTimelinePlaying", "number", ["number", "number"], [playerId, label]);
	},
	Native_EmotePlayer_IsTimelinePlaying__deps: ['EMS_EmotePlayer_IsTimelinePlaying'],

	//----------------------------------------------------------------------
	Native_EmotePlayer_CountVariables : function(playerId) {
		// console.log("EmotePlayer_CountVariables:playerId=%d", playerId);
		return ccall("EMS_EmotePlayer_CountVariables", "number", ["number"], [playerId]);
	},
	Native_EmotePlayer_CountVariables__deps: ['EMS_EmotePlayer_CountVariables'],

	//----------------------------------------------------------------------
	Native_EmotePlayer_GetVariableLabelAt : function(playerId, variableIndex) {
		// console.log("EmotePlayer_GetVariableLabelAt:playerId=%d, variableIndex=%d", playerId, variableIndex);
		return ccall("EMS_EmotePlayer_GetVariableLabelAt", "number", ["number", "number"], [playerId, variableIndex]);
	},
	Native_EmotePlayer_GetVariableLabelAt__deps: ['EMS_EmotePlayer_GetVariableLabelAt'],

	//----------------------------------------------------------------------
	Native_EmotePlayer_CountVariableFrameAt : function(playerId, variableIndex) {
		// console.log("EmotePlayer_CountVariableFrameAt:playerId=%d, variableIndex=%d", playerId, variableIndex);
		return ccall("EMS_EmotePlayer_CountVariableFrameAt", "number", ["number", "number"], [playerId, variableIndex]);
	},
	Native_EmotePlayer_CountVariableFrameAt__deps: ['EMS_EmotePlayer_CountVariableFrameAt'],

	//----------------------------------------------------------------------
	Native_EmotePlayer_GetVariableFrameLabelAt : function(playerId, variableIndex, frameIndex) {
		// console.log("EmotePlayer_GetVariableFrameLabelAt:playerId=%d, variableIndex=%d, frameIndex=%d", playerId, variableIndex, frameIndex);
		return ccall("EMS_EmotePlayer_GetVariableFrameLabelAt", "number", ["number", "number", "number"], [playerId, variableIndex, frameIndex]);
	},
	Native_EmotePlayer_GetVariableFrameLabelAt__deps: ['EMS_EmotePlayer_GetVariableFrameLabelAt'],

	//----------------------------------------------------------------------
	Native_EmotePlayer_GetVariableFrameValueAt : function(playerId, variableIndex, frameIndex) {
		// console.log("EmotePlayer_GetVariableFrameValueAt:playerId=%d, variableIndex=%d, frameIndex=%d", playerId, variableIndex, frameIndex);
		return ccall("EMS_EmotePlayer_GetVariableFrameValueAt", "number", ["number", "number", "number"], [playerId, variableIndex, frameIndex]);
	},
	Native_EmotePlayer_GetVariableFrameValueAt__deps: ['EMS_EmotePlayer_GetVariableFrameValueAt'],

	//----------------------------------------------------------------------
	Native_EmotePlayer_AddUserDefinedVariable : function(playerId, label) {
		// console.log("EmotePlayer_AddUserDefinedVariable:playerId=%d, label=[%s]", playerId, label);
		return ccall("EMS_EmotePlayer_AddUserDefinedVariable", "number", ["number", "number"], [playerId, label]);
	},
	Native_EmotePlayer_AddUserDefinedVariable__deps: ['EMS_EmotePlayer_AddUserDefinedVariable'],

	//----------------------------------------------------------------------
	Native_EmotePlayer_SetVariable : function(playerId, label, value, frameCount, easing) {
		// console.log("EmotePlayer_SetVariable:playerId=%d, label=[%s], value=%f, frameCount=%f, easing=%f", playerId, label, value, frameCount, easing);
		return ccall("EMS_EmotePlayer_SetVariable", "number", ["number", "number", "number", "number", "number"], [playerId, label, value, frameCount, easing]);
	},
	Native_EmotePlayer_SetVariable__deps: ['EMS_EmotePlayer_SetVariable'],

	//----------------------------------------------------------------------
	Native_EmotePlayer_GetVariable : function(playerId, label) {
		// console.log("EmotePlayer_GetVariable:playerId=%d, label=[%s]", playerId, label);
		return ccall("EMS_EmotePlayer_GetVariable", "number", ["number", "number"], [playerId, label]);
	},
	Native_EmotePlayer_GetVariable__deps: ['EMS_EmotePlayer_GetVariable'],

	//----------------------------------------------------------------------
	Native_EmotePlayer_SetVariableDiff : function(playerId, module, label, value, frameCount, easing) {
		// console.log("EmotePlayer_SetVariableDiff:playerId=%d, module=[%s], label=[%s], value=%f, frameCount=%f, easing=%f", playerId, module, label, value, frameCount, easing);
		return ccall("EMS_EmotePlayer_SetVariableDiff", "number", ["number", "number", "number", "number", "number", "number"], [playerId, module, label, value, frameCount, easing]);
	},
	Native_EmotePlayer_SetVariableDiff__deps: ['EMS_EmotePlayer_SetVariableDiff'],

	//----------------------------------------------------------------------
	Native_EmotePlayer_GetVariableDiff : function(playerId, module, label) {
		// console.log("EmotePlayer_GetVariableDiff:playerId=%d, module=[%s], label=[%s]", playerId, module, label);
		return ccall("EMS_EmotePlayer_GetVariableDiff", "number", ["number", "number", "number"], [playerId, module, label]);
	},
	Native_EmotePlayer_GetVariableDiff__deps: ['EMS_EmotePlayer_GetVariableDiff'],

	//----------------------------------------------------------------------
	Native_EmotePlayer_SetOuterForce : function(playerId, label, ofx, ofy, frameCount, easing) {
		// console.log("EmotePlayer_SetOuterForce:playerId=%d, label=[%s], ofx=%f, ofy=%f, frameCount=%f, easing=%f", playerId, label, ofx, ofy, frameCount, easing);
		return ccall("EMS_EmotePlayer_SetOuterForce", "number", ["number", "number", "number", "number", "number", "number"], [playerId, label, ofx, ofy, frameCount, easing]);
	},
	Native_EmotePlayer_SetOuterForce__deps: ['EMS_EmotePlayer_SetOuterForce'],

	//----------------------------------------------------------------------
	Native_EmotePlayer_SetOuterRot : function(playerId, rot, frameCount, easing) {
		// console.log("EmotePlayer_SetOuterRot:playerId=%d, rot=%f, frameCount=%f, easing=%f", playerId, rot, frameCount, easing);
		return ccall("EMS_EmotePlayer_SetOuterRot", "number", ["number", "number", "number", "number"], [playerId, rot, frameCount, easing]);
	},
	Native_EmotePlayer_SetOuterRot__deps: ['EMS_EmotePlayer_SetOuterRot'],

	//----------------------------------------------------------------------
	Native_EmotePlayer_StartWind : function(playerId, start, goal, speed, powMin, powMax) {
		// console.log("EmotePlayer_StartWind:playerId=%d, start=%f, goal=%f, speed=%f, powMin=%f, powMax=%f", playerId, start, goal, speed, powMin, powMax);
		return ccall("EMS_EmotePlayer_StartWind", "number", ["number", "number", "number", "number", "number", "number"], [playerId, start, goal, speed, powMin, powMax]);
	},
	Native_EmotePlayer_StartWind__deps: ['EMS_EmotePlayer_StartWind'],

	//----------------------------------------------------------------------
	Native_EmotePlayer_StopWind : function(playerId) {
		// console.log("EmotePlayer_StopWind:playerId=%d", playerId);
		return ccall("EMS_EmotePlayer_StopWind", "number", ["number"], [playerId]);
	},
	Native_EmotePlayer_StopWind__deps: ['EMS_EmotePlayer_StopWind'],

	//----------------------------------------------------------------------
	Native_EmotePlayer_CountPlayingTimelines : function(playerId) {
		// console.log("EmotePlayer_CountPlayingTimelines:playerId=%d", playerId);
		return ccall("EMS_EmotePlayer_CountPlayingTimelines", "number", ["number"], [playerId]);
	},
	Native_EmotePlayer_CountPlayingTimelines__deps: ['EMS_EmotePlayer_CountPlayingTimelines'],

	//----------------------------------------------------------------------
	Native_EmotePlayer_GetPlayingTimelineLabelAt : function(playerId, index) {
		// console.log("EmotePlayer_GetPlayingTimelineLabelAt:playerId=%d, index=%d", playerId, index);
		return ccall("EMS_EmotePlayer_GetPlayingTimelineLabelAt", "number", ["number", "number"], [playerId, index]);
	},
	Native_EmotePlayer_GetPlayingTimelineLabelAt__deps: ['EMS_EmotePlayer_GetPlayingTimelineLabelAt'],

	//----------------------------------------------------------------------
	Native_EmotePlayer_GetPlayingTimelineFlagsAt : function(playerId, index) {
		// console.log("EmotePlayer_GetPlayingTimelineFlagsAt:playerId=%d, index=%d", playerId, index);
		return ccall("EMS_EmotePlayer_GetPlayingTimelineFlagsAt", "number", ["number", "number"], [playerId, index]);
	},
	Native_EmotePlayer_GetPlayingTimelineFlagsAt__deps: ['EMS_EmotePlayer_GetPlayingTimelineFlagsAt'],

	//----------------------------------------------------------------------
	Native_EmotePlayer_IsAnimating : function(playerId) {
		// console.log("EmotePlayer_IsAnimating:playerId=%d", playerId);
		return ccall("EMS_EmotePlayer_IsAnimating", "number", ["number"], [playerId]);
	},
	Native_EmotePlayer_IsAnimating__deps: ['EMS_EmotePlayer_IsAnimating'],

	//----------------------------------------------------------------------
	Native_EmotePlayer_IsCharaProfileAvailable : function(playerId) {
		// console.log("EmotePlayer_IsCharaProfileAvailable:playerId=%d", playerId);
		return ccall("EMS_EmotePlayer_IsCharaProfileAvailable", "number", ["number"], [playerId]);
	},
	Native_EmotePlayer_IsCharaProfileAvailable__deps: ['EMS_EmotePlayer_IsCharaProfileAvailable'],

	//----------------------------------------------------------------------
	Native_EmotePlayer_GetCharaHeight : function(playerId) {
		// console.log("EmotePlayer_GetCharaHeight:playerId=%d", playerId);
		return ccall("EMS_EmotePlayer_GetCharaHeight", "number", ["number"], [playerId]);
	},
	Native_EmotePlayer_GetCharaHeight__deps: ['EMS_EmotePlayer_GetCharaHeight'],

	//----------------------------------------------------------------------
	Native_EmotePlayer_CountCharaProfiles : function(playerId) {
		// console.log("EmotePlayer_CountCharaProfiles:playerId=%d", playerId);
		return ccall("EMS_EmotePlayer_CountCharaProfiles", "number", ["number"], [playerId]);
	},
	Native_EmotePlayer_CountCharaProfiles__deps: ['EMS_EmotePlayer_CountCharaProfiles'],

	//----------------------------------------------------------------------
	Native_EmotePlayer_GetCharaProfileLabelAt : function(playerId, profileIndex) {
		// console.log("EmotePlayer_GetCharaProfileLabelAt:playerId=%d, profileIndex=%d", playerId, profileIndex);
		return ccall("EMS_EmotePlayer_GetCharaProfileLabelAt", "number", ["number", "number"], [playerId, profileIndex]);
	},
	Native_EmotePlayer_GetCharaProfileLabelAt__deps: ['EMS_EmotePlayer_GetCharaProfileLabelAt'],

	//----------------------------------------------------------------------
	Native_EmotePlayer_GetCharaProfile : function(playerId, label) {
		// console.log("EmotePlayer_GetCharaProfile:playerId=%d, label=[%s]", playerId, label);
		return ccall("EMS_EmotePlayer_GetCharaProfile", "number", ["number", "number"], [playerId, label]);
	},
	Native_EmotePlayer_GetCharaProfile__deps: ['EMS_EmotePlayer_GetCharaProfile'],

	//----------------------------------------------------------------------
	Native_EmotePlayer_SetStereovisionEnabled : function(playerId, toggle) {
		// console.log("EmotePlayer_SetStereovisionEnabled:playerId=%d, toggle=%d", playerId, toggle);
		return ccall("EMS_EmotePlayer_SetStereovisionEnabled", "number", ["number", "number"], [playerId, toggle]);
	},
	Native_EmotePlayer_SetStereovisionEnabled__deps: ['EMS_EmotePlayer_SetStereovisionEnabled'],

	//----------------------------------------------------------------------
	Native_EmotePlayer_SetStereovisionVolume : function(playerId, volume) {
		// console.log("EmotePlayer_SetStereovisionVolume:playerId=%d, volume=%f", playerId, volume);
		return ccall("EMS_EmotePlayer_SetStereovisionVolume", "number", ["number", "number"], [playerId, volume]);
	},
	Native_EmotePlayer_SetStereovisionVolume__deps: ['EMS_EmotePlayer_SetStereovisionVolume'],

	//----------------------------------------------------------------------
	Native_EmotePlayer_SetStereovisionParallaxRatio : function(playerId, ratio) {
		// console.log("EmotePlayer_SetStereovisionParallaxRatio:playerId=%d, ratio=%f", playerId, ratio);
		return ccall("EMS_EmotePlayer_SetStereovisionParallaxRatio", "number", ["number", "number"], [playerId, ratio]);
	},
	Native_EmotePlayer_SetStereovisionParallaxRatio__deps: ['EMS_EmotePlayer_SetStereovisionParallaxRatio'],

	//----------------------------------------------------------------------
	Native_EmotePlayer_SetStereovisionRenderScreen : function(playerId, index) {
		// console.log("EmotePlayer_SetStereovisionRenderScreen:playerId=%d, index=%d", playerId, index);
		return ccall("EMS_EmotePlayer_SetStereovisionRenderScreen", "number", ["number", "number"], [playerId, index]);
	},
	Native_EmotePlayer_SetStereovisionRenderScreen__deps: ['EMS_EmotePlayer_SetStereovisionRenderScreen'],

	//----------------------------------------------------------------------
	Native_EmotePlayer_StartRecordAPILog : function(playerId) {
		// console.log("EmotePlayer_StartRecordAPILog:playerId=%d", playerId);
		return ccall("EMS_EmotePlayer_StartRecordAPILog", "number", ["number"], [playerId]);
	},
	Native_EmotePlayer_StartRecordAPILog__deps: ['EMS_EmotePlayer_StartRecordAPILog'],

	//----------------------------------------------------------------------
	Native_EmotePlayer_StopRecordAPILog : function(playerId) {
		// console.log("EmotePlayer_StopRecordAPILog:playerId=%d", playerId);
		return ccall("EMS_EmotePlayer_StopRecordAPILog", "number", ["number"], [playerId]);
	},
	Native_EmotePlayer_StopRecordAPILog__deps: ['EMS_EmotePlayer_StopRecordAPILog'],

	//----------------------------------------------------------------------
	Native_EmotePlayer_IsRecordingAPILog : function(playerId) {
		// console.log("EmotePlayer_IsRecordingAPILog:playerId=%d", playerId);
		return ccall("EMS_EmotePlayer_IsRecordingAPILog", "number", ["number"], [playerId]);
	},
	Native_EmotePlayer_IsRecordingAPILog__deps: ['EMS_EmotePlayer_IsRecordingAPILog'],

	//----------------------------------------------------------------------
	Native_EmotePlayer_StartReplayAPILog : function(playerId) {
		// console.log("EmotePlayer_StartReplayAPILog:playerId=%d", playerId);
		return ccall("EMS_EmotePlayer_StartReplayAPILog", "number", ["number"], [playerId]);
	},
	Native_EmotePlayer_StartReplayAPILog__deps: ['EMS_EmotePlayer_StartReplayAPILog'],

	//----------------------------------------------------------------------
	Native_EmotePlayer_StopReplayAPILog : function(playerId) {
		// console.log("EmotePlayer_StopReplayAPILog:playerId=%d", playerId);
		return ccall("EMS_EmotePlayer_StopReplayAPILog", "number", ["number"], [playerId]);
	},
	Native_EmotePlayer_StopReplayAPILog__deps: ['EMS_EmotePlayer_StopReplayAPILog'],

	//----------------------------------------------------------------------
	Native_EmotePlayer_IsReplayingAPILog : function(playerId) {
		// console.log("EmotePlayer_IsReplayingAPILog:playerId=%d", playerId);
		return ccall("EMS_EmotePlayer_IsReplayingAPILog", "number", ["number"], [playerId]);
	},
	Native_EmotePlayer_IsReplayingAPILog__deps: ['EMS_EmotePlayer_IsReplayingAPILog'],

	//----------------------------------------------------------------------
	Native_EmotePlayer_ClearAPILog : function(playerId) {
		// console.log("EmotePlayer_ClearAPILog:playerId=%d", playerId);
		return ccall("EMS_EmotePlayer_ClearAPILog", "number", ["number"], [playerId]);
	},
	Native_EmotePlayer_ClearAPILog__deps: ['EMS_EmotePlayer_ClearAPILog'],

	//----------------------------------------------------------------------
	Native_EmotePlayer_GetAPILog : function(playerId) {
		// console.log("EmotePlayer_GetAPILog:playerId=%d", playerId);
		return ccall("EMS_EmotePlayer_GetAPILog", "number", ["number"], [playerId]);
	},
	Native_EmotePlayer_GetAPILog__deps: ['EMS_EmotePlayer_GetAPILog'],

	//----------------------------------------------------------------------
	Native_EmotePlayer_SetAPILog : function(playerId, log) {
		// console.log("EmotePlayer_SetAPILog:playerId=%d, log=[%s]", playerId, log);
		return ccall("EMS_EmotePlayer_SetAPILog", "number", ["number", "number"], [playerId, log]);
	},
	Native_EmotePlayer_SetAPILog__deps: ['EMS_EmotePlayer_SetAPILog'],

	//----------------------------------------------------------------------
	Native_EmotePlayer_GetSuitableClearColor : function(playerId) {
		// console.log("EmotePlayer_GetSuitableClearColor:playerId=%d", playerId);
		return ccall("EMS_EmotePlayer_GetSuitableClearColor", "number", ["number"], [playerId]);
	},
	Native_EmotePlayer_GetSuitableClearColor__deps: ['EMS_EmotePlayer_GetSuitableClearColor'],

};

mergeInto(LibraryManager.library, EmotePlayerJS);


// [EOF]
