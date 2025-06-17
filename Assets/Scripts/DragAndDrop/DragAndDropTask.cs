using NodeCanvas.Framework;
using ParadoxNotion.Design;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions{

	public class DragAndDropTask : ActionTask{


        [RequiredField]
        [BlackboardOnly, Tooltip("List of draggable objects that is being checked")]
        public BBParameter<List<Answer>> Answers;

		private bool taskCompleted = false;

		/// <summary>
		/// Number of correct answers within the list
		/// </summary>
		private int correctCount = 0;

        //Use for initialization. This is called only once in the lifetime of the task.
        //Return null if init was successfull. Return an error string otherwise
        protected override string OnInit(){
            Answer.onAnswerDroppedSuccessfully += CheckTheAnswers;
            return null;
		}

		//This is called once each time the task is enabled.
		//Call EndAction() to mark the action as finished, either in success or failure.
		//EndAction can be called from anywhere.
		protected override void OnExecute(){
            //Update correct count
            foreach (Answer answer in Answers.value)
			{
				if (answer.CorrectAnswer)
				{
					correctCount++;
				}
			}
		}


		//Called once per frame while the action is active.
		protected override void OnUpdate(){

			if (taskCompleted && PrecisionMachiningGUI.Instance.FeedbackDismissed)
            {
                EndAction(true);
                correctCount = 0;
            }

        }

        //Called when the task is disabled.
        protected override void OnStop(){
            Answer.onAnswerDroppedSuccessfully -= CheckTheAnswers;
        }

        //Called when the task is paused.
        protected override void OnPause(){
			
		}

        private void CheckTheAnswers()
        {
			//Continously check which answers have been completed
			int completeCount = 0;
			foreach (Answer answer in Answers.value)
			{
				if (answer.Complete)
				{
					completeCount++;
				}
			}

            //Once all correct answers have been completed and the feedback button has been clicled, end the task
            if (completeCount == correctCount)
			{
				taskCompleted = true;
			}
		}
	}
}