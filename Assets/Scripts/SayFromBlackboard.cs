using NodeCanvas.DialogueTrees;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;
using ParadoxNotion;


namespace Dialogue{

	[Category("Dialogue")]
	[ParadoxNotion.Design.Icon("Dialogue")]
	public class SayFromBlackboard : ActionTask<IDialogueActor>{

		[RequiredField]
		public BBParameter<Answer> answer;
		private string subtitles;
		private AudioClip voiceOver;
		protected override void OnExecute()
		{
            subtitles = answer.value.FeedbackMessage;
            voiceOver = answer.value.AudioClip;
            Statement statement = new Statement(subtitles, voiceOver);
            DialogueTree.RequestSubtitles(new SubtitlesRequestInfo(agent, statement, EndAction));
        }
	}
}