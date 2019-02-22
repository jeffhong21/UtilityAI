namespace AtlasAI.AIEditor
{
    using UnityEngine;
    using UnityEditor;



    public static class EditorStyling
    {
        //Resources.Load<Texture2D>("xnode_dot")
        public static Texture2D HeaderBackground = Resources.Load<Texture2D>("HeaderBackground"); 
        public static Texture2D SelectorBackground = Resources.Load<Texture2D>("SelectorBackground"); 
        public static Texture2D SelectorSelectedBackground = Resources.Load<Texture2D>("SelectorSelectedBackground"); 
        public static Texture2D QualifierBackground = Resources.Load<Texture2D>("QualifierBackground"); 
        public static Texture2D QualifierSelectedBackground = Resources.Load<Texture2D>("QualifierSelectedBackground"); 
        public static Texture2D ActionBackground = Resources.Load<Texture2D>("ActionBackground"); 

        public static Texture2D ExpandIcon = Resources.Load<Texture2D>("ExpandIcon"); 
        public static Texture2D ConnectorOpen = Resources.Load<Texture2D>("ConnectorOpen"); 


        public static Texture2D GraphBackground = Resources.Load<Texture2D>("GraphBackground"); 




        private const int defaultFontSize = 11;

        public static readonly GUIContent addTooltip = new GUIContent()
        {
            tooltip = "Add new options content",
            image = EditorGUIUtility.IconContent("Toolbar Plus").image as Texture2D
        };
        public static readonly GUIContent deleteTooltip = new GUIContent()
        {
            tooltip = "Remove option content",
            image = EditorGUIUtility.IconContent("Toolbar Minus").image as Texture2D
        };
        public static readonly GUIContent changeTypeTooltip = new GUIContent()
        {
            tooltip = "Change options content",
            image = EditorGUIUtility.IconContent("preAudioLoopOff").image as Texture2D
        };

        private static readonly Color defaultTextColor = Color.black;
        private static readonly GUIStyle smallButtonBase = new GUIStyle(EditorStyles.miniButton);



        /// <summary>
        /// Anything that is on the AIEditor Canvas
        /// </summary>
        public static class Canvas
        {
            public static GUIStyle normalHeader { get; private set; }
            public static GUIStyle normalSelector { get; private set; }
            public static GUIStyle activeSelector { get; private set; }
            public static GUIStyle normalQualifier { get; private set; }
            public static GUIStyle normalAction { get; private set; }


            public static void Initialize()
            {
                normalHeader = new GUIStyle();
                normalHeader.border = new RectOffset(12, 12, 12, 12);

                normalSelector = new GUIStyle();
                normalSelector.border = new RectOffset(12, 12, 12, 12);

                activeSelector = new GUIStyle();
                activeSelector.border = new RectOffset(12, 12, 12, 12);

                normalQualifier = new GUIStyle();
                normalQualifier.border = new RectOffset(12, 12, 12, 12);

                normalAction = new GUIStyle();
                normalAction.border = new RectOffset(12, 12, 12, 12);
            }

            public static void SetTextures()
            {
                normalHeader.normal.background = HeaderBackground;
                normalSelector.normal.background = SelectorBackground;
                activeSelector.normal.background = SelectorSelectedBackground;
                normalQualifier.normal.background = QualifierBackground;
                normalAction.normal.background = ActionBackground;
            }

            static Canvas()
            {
                Initialize();
                SetTextures();
            }
        }


        public static class NodeStyles
        {
            public static GUIStyle nodeTitle { get; private set; }
            public static GUIStyle nodeTitleActive { get; private set; }
            public static GUIStyle normalBoxText { get; private set; }
            public static GUIStyle activeBoxText { get; private set; }


            public static void Initialize()
            {
                nodeTitle = new GUIStyle();
                nodeTitle.normal.textColor = Color.white;
                //nodeTitle.fontStyle = FontStyle.Normal;
                nodeTitle.alignment = TextAnchor.MiddleCenter;

                nodeTitleActive = new GUIStyle();
                nodeTitleActive.normal.textColor = new Color(0f, 0.6f, 1f, 1f);
                nodeTitleActive.fontStyle = FontStyle.Bold;
                nodeTitleActive.alignment = TextAnchor.MiddleCenter;

                normalBoxText = new GUIStyle();
                normalBoxText.normal.textColor = Color.white;
                //normalBoxText.fontStyle = FontStyle.Normal;
                normalBoxText.alignment = TextAnchor.MiddleLeft;
                normalBoxText.contentOffset = new Vector2(25, 0);

                activeBoxText = new GUIStyle();
                activeBoxText.normal.textColor = new Color(0f, 0.6f, 1f, 1f);
                activeBoxText.fontStyle = FontStyle.Bold;
                activeBoxText.alignment = TextAnchor.MiddleLeft;
                activeBoxText.contentOffset = new Vector2(25, 0);
            }



            static NodeStyles()
            {
                Initialize();
            }
        }



        /// <summary>
        /// Anything inspector related.
        /// </summary>
        public static class Skinned
        {
            public static GUIStyle boldTitle { get; private set; }
            public static GUIStyle inspectorTitle { get; private set; }

            public static GUIStyle addButtonSmall { get; private set; }
            public static GUIStyle deleteButtonSmall { get; private set; }
            public static GUIStyle changeButtonSmall { get; private set; }



            public static void Initialize()
            {
                boldTitle = new GUIStyle();
                boldTitle.fontStyle = FontStyle.Bold;
                boldTitle.alignment = TextAnchor.MiddleCenter;
                boldTitle.contentOffset = new Vector2(0, 2);

                inspectorTitle = new GUIStyle();
                inspectorTitle.fontSize = 12;
                inspectorTitle.fontStyle = FontStyle.Bold;


                addButtonSmall = new GUIStyle(smallButtonBase);
                deleteButtonSmall = new GUIStyle(smallButtonBase);
                changeButtonSmall = new GUIStyle(smallButtonBase);
            }


            public static void SetTextures()
            {

            }

            static Skinned()
            {
                Initialize();
                SetTextures();
            }
        }





        #region OLD STYLES

        //public static class Icons
        //{
        //    public static Texture2D DeleteIcon;
        //    public static Texture2D ChangeIcon;
        //    public static Texture2D AddIcon;

        //    static Icons()
        //    {
        //        DeleteIcon = EditorGUIUtility.IconContent("Toolbar Minus").image as Texture2D;
        //        ChangeIcon = EditorGUIUtility.IconContent("preAudioLoopOff").image as Texture2D;
        //        AddIcon = EditorGUIUtility.IconContent("Toolbar Plus").image as Texture2D;
        //    }
        //}

        //public static class Styles
        //{
        //    //  Node Styles.
        //    public static GUIStyle defaultNodeStyle;

        //    //  Text Styles.
        //    public static GUIStyle TextCenterStyle;
        //    public static GUIStyle DebugTextStyle;
        //    //  Node Style
        //    public static GUIStyle nodeStyle;
        //    public static GUIStyle selectedNodeStyle;

        //    static Styles()
        //    {

        //        defaultNodeStyle = new GUIStyle
        //        {
        //            fontStyle = FontStyle.Bold,
        //            alignment = TextAnchor.MiddleLeft,
        //            contentOffset = new Vector2(25, -3)
        //        };


        //        TextCenterStyle = new GUIStyle
        //        {
        //            alignment = TextAnchor.MiddleCenter
        //        };

        //        DebugTextStyle = new GUIStyle
        //        {
        //            richText = true
        //        };


        //        nodeStyle = new GUIStyle("TL SelectionButton PreDropGlow")
        //        {
        //            fontStyle = FontStyle.Bold,
        //            alignment = TextAnchor.MiddleLeft,
        //        };

        //        selectedNodeStyle = new GUIStyle("TL SelectionButton PreDropGlow")
        //        {
        //            fontStyle = FontStyle.Bold,
        //            alignment = TextAnchor.MiddleLeft,
        //        };


        //    }
        //}

        # endregion
    }





}


