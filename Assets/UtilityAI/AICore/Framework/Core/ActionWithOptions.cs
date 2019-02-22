using System;
using System.Collections.Generic;

namespace AtlasAI
{
    public abstract class ActionWithOptions<TOption> : IAction
    {
        //
        // Fields
        //
        protected List<IOptionScorer<TOption>> _scorers;

        //
        // Properties
        //
        public List<IOptionScorer<TOption>> scorers
        {
            get { return _scorers; }
            set { _scorers = value; }
        }

        //
        // Constructors
        //
        protected ActionWithOptions(){}

        //
        // Methods
        //

        public void Execute(IAIContext context)
        {
            OnExecute(context);
        }





        public TOption GetBest(IAIContext context, List<TOption> options)
        {
            //  Get buffer list from pool.
            var optionsBuffer = Utilities.ListBufferPool.GetBuffer<ScoredOption<TOption>>(options.Count);
            //UnityEngine.Debug.LogFormat("OptionsBuffer<{2}>: Count: {0} | Capacity: {1}", optionsBuffer.Count, optionsBuffer.Capacity, typeof(TOption));

            GetAllScores(context, options, optionsBuffer);
            //UnityEngine.Debug.LogFormat("OptionsBuffer<{2}>: Count: {0} | Capacity: {1}", optionsBuffer.Count, optionsBuffer.Capacity, typeof(TOption));
            //  Sort list.
            optionsBuffer.Sort(new ScoredOptionComparer<TOption>());
            var bestOption = optionsBuffer[0].option;
            //UnityEngine.Debug.LogFormat("Best: Score: {0},  Option {1}    |    Worst Score: {2},  Option {3} ", optionsBuffer[0].score, optionsBuffer[0].option, optionsBuffer[optionsBuffer.Count-1].score, optionsBuffer[optionsBuffer.Count - 1].option);
            //  Return bufer list to pool.
            //DebugOptions(optionsBuffer, 4);
            Utilities.ListBufferPool.ReturnBuffer<ScoredOption<TOption>>(optionsBuffer);
            return bestOption;
        }


        ///<summary>
        /// Gets all options with the score they received from the<see cref= "P:Apex.AI.ActionWithOptions`1.scorers" />.
        /// optionBuffer parameter is a list from a ListBufferPool
        ///</ summary >
        ///< param name= "context" > The context.</param>
        ///<param name = "options" > The options.</param>
        ///<param name = "optionsBuffer" > The buffer which is populated with the scored options.</param>
        public void GetAllScores(IAIContext context, List<TOption> options, List<ScoredOption<TOption>> optionsBuffer)
        {
            for (int i = 0; i < options.Count; i++){
                //  For each option, loop through all the option scorers.
                var option = options[i];
                var score = 0f;
                //  Looop through each scorers and process score.
                for (int index = 0; index < scorers.Count; index++)
                    score += scorers[index].Score(context, option);
                
                optionsBuffer.Insert(i, new ScoredOption<TOption>(option, score));
            }
        }



        public abstract void OnExecute(IAIContext context);










        private void DebugOptions(List<ScoredOption<TOption>> options, int perLine)
        {
            string info = string.Format("Scored Options | type: {0}, count: {1} \n ",typeof(TOption), options.Count );
            string newLine = "\n";
            int count = 0;
            for (int i = 0; i < options.Count; i++){
                newLine = (count % perLine == 0) ? "\n" : "  |  ";
                info += string.Format("({0}): option : {1}, score : {2} (remainder: {3})", i, options[i].option, options[i].score, count % perLine);
                info += newLine;
                count++;
            }
            UnityEngine.Debug.Log(info);
        }















//#if UNITY_EDITOR
//        public KeyValuePair<Vector3, float>[] GetScoredOptions(IAIContext context)
//        {
//            var c = context as AgentContext;
//            var allScores = ListBufferPool.GetBuffer<ScoredOption>(c.sampledPositions.Count);
//            this.GetAllScorers(context, c.sampledPositions, allScores);

//            var scoredOptions = (from score in allScores
//                                 select new KeyValuePair<Vector3, float>(score.option, score.score)).ToArray();

//            ListBufferPool.ReturnBuffer<ScoredOption>(allScores);

//            return scoredOptions;
//        }
//#endif


        //public TOption GetBest(IAIContext context, List<TOption> options)
        //{
        //    List<ScoredOption<TOption>> scoredOptions = GetAllScores(context, options);
        //    scoredOptions.Sort(new ScoredOptionComparer<TOption>());
        //    return scoredOptions[0].option;
        //}

        /////<summary>
        ///// Gets all options with the score they received from the<see cref= "P:Apex.AI.ActionWithOptions`1.scorers" />.
        ///// optionBuffer parameter is a list from a ListBufferPool
        /////</ summary >
        /////< param name= "context" > The context.</param>
        /////<param name = "options" > The options.</param>
        /////<param name = "optionsBuffer" > The buffer which is populated with the scored options.</param>
        //public List<ScoredOption<TOption>> GetAllScores(IAIContext context, List<TOption> options)
        //{
        //    List<ScoredOption<TOption>> scoredOptions = new List<ScoredOption<TOption>>();

        //    for (int i = 0; i < options.Count; i++)
        //    {
        //        //  For each option, loop through all the option scorers.
        //        var option = options[i];
        //        var score = 0f;

        //        for (int index = 0; index < scorers.Count; index++)
        //            score += scorers[index].Score(context, option);
                
        //        ScoredOption<TOption> scoredOption = new ScoredOption<TOption>(option, score);
        //        scoredOptions.Add(scoredOption);
        //    }

        //    return scoredOptions;
        //}




    }
}
