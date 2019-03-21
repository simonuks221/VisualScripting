using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace VisualScripting
{
    public class ProjectManager
    {
        Panel navigationPanel;

        public VisualProject visualProject;

        public List<BaseEditorManager> showingEditors;
        public int currentEditorIndex = 0;

        Form1 form;

        public ProjectManager(Form1 _form)
        {
            form = _form;
            form.projectManager = this;
            navigationPanel = form.NavigationPanel;

            visualProject = new VisualProject();
            showingEditors = new List<BaseEditorManager>();

            AssetsEditorManager assetEditor = new AssetsEditorManager(form);
            showingEditors.Add(assetEditor);

            VisualClass newClass = new VisualClass("Program");
            VisualClassScriptEditorManager scriptEdditor = new VisualClassScriptEditorManager(form, newClass);
            showingEditors.Add(scriptEdditor);
            visualProject.visualClasses.Add(newClass);

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

                if(i == currentEditorIndex)
                {
                    newPanel.BackColor = Color.LightYellow;
                }
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

        public void ChangeSelectedEditorIndex(int _newIndex)
        {
            currentEditorIndex = _newIndex;

            int numberOfControls = form.MainScriptingPanel.Controls.Count;
            for(int i = 0; i < numberOfControls; i++)
            {
                form.MainScriptingPanel.Controls[i].Hide();
            }

            showingEditors[currentEditorIndex].DisplayAllOnMainPanel();

            /*
            var checkCodeEditor = showingEditors[currentEditorIndex] as VisualScriptEditorManager;
            var checkAssetEditor = showingEditors[currentEditorIndex] as AssetsEditorManager;

            if (checkCodeEditor != null) //Editing class
            {
                VisualScriptEditorManager editor = checkCodeEditor as VisualScriptEditorManager;
                editor.DisplayAllOnMainPanel();
            }
            if(checkAssetEditor != null)
            {
                
            }*/

            UpdateNavigationPanel();
        }

        public void AddNewClass()
        {
            VisualClass newVisualClass = new VisualClass("naujaKlase");
            visualProject.visualClasses.Add(newVisualClass);

            AddNewShowingEditor(newVisualClass);

            ChangeSelectedEditorIndex(showingEditors.Count - 1);
        }

        public void AddNewShowingEditor(VisualBase _visualBase)
        {
            var CheckClass = _visualBase as VisualClass;

            if (CheckClass != null)
            {
                VisualClassScriptEditorManager newEditorManager = new VisualClassScriptEditorManager(form, (VisualClass)_visualBase);
                showingEditors.Add(newEditorManager);
            }

            UpdateNavigationPanel();
        }

        public void CompileAllToString()
        {
            string allCode = @"
using System; 
using System.Collections.Generic; 
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VisualScripting;

namespace VisualScripting
{ ";
            allCode += "\n";
            for (int i = 0; i < visualProject.visualClasses.Count; i++)
            {
                allCode += visualProject.visualClasses[i].CompileToString() + " \n ";
            }

    
allCode += "}";
            Console.Out.WriteLine(allCode);
            if (ConsoleForm.Instance == null)
            {
                ConsoleForm c = new ConsoleForm();
                c.Show();
            }

            VisualScriptCompiler visualCompiler = new VisualScriptCompiler(allCode, this);
            visualCompiler = null;
        }
    }
}
