//namespace AtlasAI
//{
//    using UnityEngine;
//    using System;
//    using System.Collections.Generic;



//    /// <summary>
//    /// action with options.
//    /// </summary>
//    [Serializable]
//    public abstract class ActionWithOptions<TOption> : IAction
//    {
//        public string name { get; set; }

//        //  All the OptionScorers attached to this action.
//        [SerializeField]
//        protected List<IOptionScorer<TOption>> _scorers;


//        public List<IOptionScorer<TOption>> scorers
//        {
//            get { return _scorers; }
//            set { _scorers = value; }
//        }


//        //
//        // Constructors
//        //
//        public ActionWithOptions()
//        {
//            scorers = new List<IOptionScorer<TOption>>();

//        }



//        /// <summary>
//        /// "Gets the best option, i.e. the option that was given the highest combined score by thr ActionWithOptions<TOption>.scorers"
//        /// Gets the TOption tha has the highest score after being scored by all the scorers.  
//        /// </summary>
//        /// <returns> Return a type option for the action to execute on..</returns>
//        /// <param name="context">Context.</param>
//        /// <param name="options">Options.</param>
//        public TOption GetBest(IAIContext context, List<TOption> options)
//        {
//            var scoredOptions = new List<ScoredOption<TOption>>();
//            //scoredOptions.Clear();

//            if (options.Count == 0)
//                return default(TOption);

//            TOption best = options[0];

//            scoredOptions = GetAllScorers(context, options, scoredOptions);

//            scoredOptions.Sort();
//            scoredOptions.Reverse();
//            best = scoredOptions[0].option;

//            return scoredOptions[0].score == 0 ? default(TOption) : scoredOptions[0].option;

//            //string scoredOptionInfo = "";
//            //foreach(ScoredOption<TOption> scoredOption in scoredOptions){
//            //    scoredOptionInfo += string.Format("OptionType:  {0}  | Score: {1}\n", scoredOption.option, scoredOption.score);
//            //}
//            //Debug.Log(scoredOptionInfo);

//            //return best;
//        }


//        /// <summary>
//        /// "Gets all options with the score they received from the ActionWithOptions<TOption>.scorers."
//        /// Instead of scoring with default method, which returns the TOption with the highest score, 
//        /// you can get a list of all the options with their scores and you can select them yourself with another logic.
//        /// </summary>
//        /// <param name="context">Context.</param>
//        /// <param name="options">Options.</param>
//        /// <param name="optionsBuffer">The buffer which is populated with the scored options.</param>
//        public List<ScoredOption<TOption>> GetAllScorers(IAIContext context, List<TOption> options, List<ScoredOption<TOption>> optionsBuffer)
//        {
//            //optionsBuffer.Clear();
//            //  Loop through every TOption and tallys all scorers.  (e.g loop through all Vector3 position points and than calculate all scores of that position.)
//            for (int i = 0; i < options.Count; i++)
//            {
//                var option = options[i];
//                float score = 0f;
//                //  Loop through each scorer options.
//                for (int index = 0; index < scorers.Count; index++){
//                    score += scorers[index].Score(context, option);
//                }
//                //  ScoredOptions would contain all the scores and TOption to return to the ActionWithOptions.
//                optionsBuffer.Add(new ScoredOption<TOption>(option, score));
//            }
//            return optionsBuffer;
//        }





//        public void CloneFrom(ActionWithOptions<TOption> other){
//            scorers = other.scorers;
//            //scoredOptions = other.scoredOptions;
//        }




//        public abstract void Execute(IAIContext context);
//    }





//}