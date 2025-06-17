using NodeCanvas.Framework;
using ParadoxNotion.Design;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions{

	public class VoiceOverTask : ActionTask{

		public delegate void OnVoiceOverRequested(VoiceOverScriptableObject section, bool repeat);
		public static OnVoiceOverRequested onVoiceOverRequested;

		//Use for initialization. This is called only once in the lifetime of the task.
		//Return null if init was successfull. Return an error string otherwise
		protected override string OnInit(){

            return null;
		}

		[RequiredField]
		public BBParameter<VoiceOverScriptableObject> voiceOverObject;

		//This is called once each time the task is enabled.
		//Call EndAction() to mark the action as finished, either in success or failure.
		//EndAction can be called from anywhere.
		protected override void OnExecute(){
			onVoiceOverRequested?.Invoke(voiceOverObject.value, false);
		}

	}
}