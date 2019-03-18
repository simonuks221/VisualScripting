using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace VisualScripting
{
    class ProjectManager
    {
        Panel navigationPanel;

        VisualProject visualProject;

        public List<BaseEditorManager> showingEditors;
        public int currentEditorIndex = 0;

        Form1 form;

        public ProjectManager(Form1 _form)
        {
            form = _form;
            navigationPanel = form.NavigationPanel;

            visualProject = new VisualProject();
            showingEditors = new List<BaseEditorManager>();

            AssetsEditorManager assetEditor = new AssetsEditorManager();
            showingEditors.Add(assetEditor);

            ChangeSelectedEditorIndex(0);
            UpdateNavigationPanel();
        }

        void UpdateNavigationPanel()
        {
            int amountOfChildren = navigationPanel.Controls.Count;
            for(int i = 0; i < amountOfChildren; i++)
            {
                navigationPanel.Controls[0].Dispose();
            }

            for (int i = 0; i < showingEditors.Count; i++)
            {
                BaseNavigationPanelPart newPanel = null;
                var checkAsset = showingEditors[i] as AssetsEditorManager;
                if (checkAsset != null) //Asset editor
                {
                    newPanel = new AssetsManagerNavigationPanelPart(i);
                }
                else //Not asset ediotr
                {
                    newPanel = new VisualClassNavigationPanelPart(i, visualProject.visualClasses[i - 1]);

                }
                navigationPanel.Controls.Add(newPanel);
                newPanel.Location = new Point(i * 105 + 2, 2);
                newPanel.navigationPanelPressed += NavigationPanelPartPressed;
                newPanel.closeButton.Click += NavigationPanelPartClose;
            }
        }

        private void NavigationPanelPartClose(object sender, EventArgs e)
        {
            Control ctr = sender as Control;
            BaseNavigationPanelPart panel = ctr.Parent as BaseNavigationPanelPart;
            showingEditors.RemoveAt(panel.navigationPanelPartIndex);

            currentEditorIndex = 0;

            UpdateNavigationPanel();
        }

        private void NavigationPanelPartPressed(BaseNavigationPanelPart panelPressed)
        {
            var checkClass = panelPressed as VisualClassNavigationPanelPart;
            var checkFunction = panelPressed as VisualFunctionNavigationPanelPart;
            var checkAsset = panelPressed as AssetsManagerNavigationPanelPart; ;

            if (checkClass != null) //Class
            {
                ChangeSelectedEditorIndex(checkClass.navigationPanelPartIndex);
            }
            else if(checkFunction != null) //Function
            {
                ChangeSelectedEditorIndex(checkFunction.navigationPanelPartIndex);
            }
            else if(checkAsset != null)
            {
                ChangeSelectedEditorIndex(checkAsset.navigationPanelPartIndex);
            }
        }

        void ChangeSelectedEditorIndex(int _newIndex)
        {
            currentEditorIndex = _newIndex;

            int numberOfControls = form.MainScriptingPanel.Controls.Count;
            for(int i = 0; i < numberOfControls; i++)
            {
                form.MainScriptingPanel.Controls[0].Hide();
                Console.Out.WriteLine("hid");
            }

            var checkCodeEditor = showingEditors[currentEditorIndex] as VisualScriptEditorManager;
            var checkAssetEditor = showingEditors[currentEditorIndex] as AssetsEditorManager;

            if (checkCodeEditor != null) //Editing class
            {
                VisualScriptEditorManager editor = checkCodeEditor as VisualScriptEditorManager;
                editor.DisplayAllNodesOnEditor();
            }
            if(checkAssetEditor != null)
            {
                
            }
        }

        public void AddnewClass()
        {
            VisualClass newVisualClass = new VisualClass("nauja klase");
            visualProject.visualClasses.Add(newVisualClass);
            VisualScriptEditorManager newEditorManager = new VisualScriptEditorManager(newVisualClass, form.MainScriptingPanel, form.VariableAndFunctionPanel, form.VariableFunctionInfoPanel);
            showingEditors.Add(newEditorManager);

            UpdateNavigationPanel();
        }
    }
}
