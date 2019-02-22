namespace AtlasAI.Visualization
{
    using UnityEngine;
    using UnityEditor;
    using System;
    using System.Collections.Generic;


    /// <summary>
    /// The final step for custom visualization. This base class covers the expected most common use case for custom visualization: 
    /// Visualizing the scores for each option used by an ActionWithOptions<TOption>. Provides an abstract method for getting the 
    /// desired options through the Context object.
    /// </summary>
    public abstract class ActionWithOptionsVisualizerComponent<T, TOption> : CustomVisualizerComponent<T, List<ScoredOption<TOption>>> where T : ActionWithOptions<TOption>
    {
        //
        // Fields
        //
        private List<ScoredOption<TOption>> _scoredBuffer = new List<ScoredOption<TOption>>();


        //
        // Methods
        //
        protected override List<ScoredOption<TOption>> GetDataForVisualization(T aiEntity, IAIContext context, Guid aiId)
        {
            _scoredBuffer = Utilities.ListBufferPool.GetBuffer<ScoredOption<TOption>>(GetOptions(context).Count);
            aiEntity.GetAllScores(context, GetOptions(context), _scoredBuffer);
            Utilities.ListBufferPool.ReturnBuffer<ScoredOption<TOption>>(_scoredBuffer);
            //Debug.Log(_scoredBuffer.Count);
            return _scoredBuffer;
        }


        protected abstract List<TOption> GetOptions(IAIContext context);
    }









}
