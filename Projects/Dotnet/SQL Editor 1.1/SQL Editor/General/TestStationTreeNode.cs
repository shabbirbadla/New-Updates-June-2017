using System;
using System.IO;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;


namespace SQLEditor.General.TestBench
{
	/// <summary>
	/// Summary description for TestStationTreeNode.
	/// </summary>
	public class TestStationTreeNode:System.Windows.Forms.TreeNode
	{
		Action _action;
		public Action action
		{
			get{return _action;}
			set
			{
				_action=value;
				this.Text=action.Name;
				
				switch(action.Type)
				{
					case ActionType.Root:
						this.ImageIndex=0;
						this.SelectedImageIndex=0;
						break;
					case ActionType.None:
						this.ImageIndex=1;
						this.SelectedImageIndex=1;
						break;
					case ActionType.Script:
						this.ImageIndex=2;
						this.SelectedImageIndex=2;
						break;
					case ActionType.Wait:
						this.ImageIndex=3;
						this.SelectedImageIndex=3;
						break;
				}
			}
		}

		public TestStationTreeNode(Action action)
		{
			//this.Text=action.Name;
			this.action=action;
//			switch(action.Type)
//			{
//				case ActionType.Root:
//					this.ImageIndex=0;
//					this.SelectedImageIndex=0;
//					break;
//				case ActionType.None:
//					this.ImageIndex=1;
//					this.SelectedImageIndex=1;
//					break;
//				case ActionType.Script:
//					this.ImageIndex=2;
//					this.SelectedImageIndex=2;
//					break;
//				case ActionType.Wait:
//					this.ImageIndex=3;
//					this.SelectedImageIndex=3;
//					break;
//			}
		}
		public void ResetImage()
		{
			foreach(TestStationTreeNode node in this.Nodes)
				node.ResetImage();

			switch(action.Type)
			{
				case ActionType.Root:
					this.ImageIndex=0;
					this.SelectedImageIndex=0;
					break;
				case ActionType.None:
					this.ImageIndex=1;
					this.SelectedImageIndex=1;
					break;
				case ActionType.Script:
					this.ImageIndex=2;
					this.SelectedImageIndex=2;
					break;
				case ActionType.Wait:
					this.ImageIndex=3;
					this.SelectedImageIndex=3;
					break;
			}
		}
		
	}
}
