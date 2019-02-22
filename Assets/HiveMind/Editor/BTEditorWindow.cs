﻿using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace Hivemind {

	public class MenuAction {

		public string nodeType;
		public Vector2 position;
		public Node node;

		public MenuAction(Node nodeVal) {
			node = nodeVal;
		}
		
		public MenuAction(Node nodeVal, Vector2 positionVal, string nodeTypeVal) {
			node = nodeVal;
			position = positionVal;
			nodeType = nodeTypeVal;
		}
	}

	public class BTEditorWindow : EditorWindow {

		public View view;
		private GUIStyle _noSelectionStyle;
		private bool _debugMode;

		[MenuItem("Window/Behavior Tree Editor")]
		public static void ShowWindow() {
			BTEditorWindow editor = EditorWindow.GetWindow<BTEditorWindow>();
			editor.minSize = new Vector2(100, 100);
			editor.title = "Behavior Tree";
		}

		void OnSelectionChange() {
			Repaint ();
		}

		void OnEnable() {
			_noSelectionStyle = new GUIStyle();
			_noSelectionStyle.fontSize = 24;
			_noSelectionStyle.alignment = TextAnchor.MiddleCenter;
			if (BTEditorManager.Manager != null) BTEditorManager.Manager.editorWindow = this;
		}

		void OnDisable() {
			if (BTEditorManager.Manager) BTEditorManager.Manager.editorWindow = null;
		}

		void OnDestroy() {
			if (BTEditorManager.Manager) BTEditorManager.Manager.editorWindow = null;
		}

		void OnGUI() {

			if (BTEditorManager.Manager != null && BTEditorManager.Manager.behaviorTree != null) {

				BTEditorManager.Manager.editorWindow = this;

				if (view == null)
					view = new View(this);

				if (view.nodeInspector != null)
					view.nodeInspector.OnInspectorGUI();

				bool viewNeedsRepaint = view.Draw(position);
				if (viewNeedsRepaint)
					Repaint ();
				
				view.ResizeCanvas();

			} else {
				GUI.Label(new Rect(0, 0, position.width, position.height), "No Behavior Tree selected", _noSelectionStyle);
			}

		}

		public void ShowContextMenu(Vector2 point, Node node) {

			if (Application.isPlaying) {
				return;
			}

			var menu = new GenericMenu();

			if (node == null || node.CanConnectChild) {

				// Add new node
				string addMsg = (node == null ? "Add" : "Add Child") + "/";

				// List all available node subclasses
				int length = BehaviorTree.NodeTypes.Length;
				for ( int i = 0; i < length; i++ ) {
					menu.AddItem (
						new GUIContent(addMsg + BehaviorTree.NodeTypes[i]),
						false,
						Add,
						new MenuAction(node, point, BehaviorTree.NodeTypes[i])
						);
				}


			} else {
				menu.AddDisabledItem(new GUIContent("Add"));
			}

			if (node == null) {
				menu.AddSeparator ("");
				menu.AddItem (new GUIContent("Save"), false, Save, null);
			}

			menu.AddSeparator ("");

			// Node actions
			if (node != null) {

				// Connect/Disconnect Parent
				if (!(node is Root)) {
					if (node.parent != null)
						menu.AddItem(new GUIContent("Disconnect from Parent"), false, Unparent, new MenuAction(node));
					else
						menu.AddItem(new GUIContent("Connect to Parent"), false, ConnectParent, new MenuAction(node));
				}

				menu.AddSeparator ("");

				// Connect Child
				if (node.CanConnectChild)
					menu.AddItem(new GUIContent("Connect to Child"), false, ConnectChild, new MenuAction(node));
				else
					menu.AddDisabledItem(new GUIContent("Connect to Child"));

				menu.AddSeparator ("");
				
				// Deleting
				if (node is Root)
					menu.AddDisabledItem(new GUIContent("Delete"));
				else
					menu.AddItem(new GUIContent("Delete"), false, Delete, new MenuAction(node));
					
			}

			menu.DropDown(new Rect(point.x, point.y, 0, 0));
		}

		// Context Menu actions

		public void Add(object userData) {
			MenuAction menuAction = userData as MenuAction;
			BTEditorManager.Manager.Add (menuAction.node, menuAction.position, menuAction.nodeType);
			view.ResizeCanvas();
			Repaint ();
		}

		public void Unparent(object userData) {
			MenuAction menuAction = userData as MenuAction;
			BTEditorManager.Manager.Unparent(menuAction.node);
			Repaint ();
		}

		public void ConnectParent(object userData) {
			MenuAction menuAction = userData as MenuAction;
			view.ConnectParent (menuAction.node);
		}

		public void ConnectChild(object userData) {
			MenuAction menuAction = userData as MenuAction;
			view.ConnectChild (menuAction.node);
		}

		public void Delete(object userData) {
			MenuAction menuAction = userData as MenuAction;
			BTEditorManager.Manager.Delete (menuAction.node);
			Repaint();
		}

		public void Save(object userData) {
			AssetDatabase.Refresh();
			AssetDatabase.SaveAssets();
			Debug.Log ("Save");
		}


	}

	
}