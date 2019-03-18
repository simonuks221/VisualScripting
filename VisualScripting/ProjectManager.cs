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

        public List<VisualScriptEditorManager> showingVisualEditors;
        public int currentEditorIndex = 0;

        Form1 form;

        public ProjectManager(Form1 _form)
        {
            form = _form;
            navigationPanel = form.NavigationPanel;

            visualProject = new VisualProject();
            showingVisualEditors = new List<VisualScriptEditorManager>();
            visualProject.visualClasses.Add(new VisualClass("Class1"));

            showingVisualEditors.Add(new VisualScriptEditorManager(form.MainScriptingPanel, form.VariableAndFunctionPanel, form.VariableFunctionInfoPanel));

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

            for (int i = 0; i < showingVisualEditors.Count; i++)
            {
                VisualClassNavigationPanelPart newPanel = new VisualClassNavigationPanelPart(i, visualProject.visualClasses[i]);
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
            showingVisualEditors.RemoveAt(panel.navigationPanelPartIndex);

            currentEditorIndex = 0;

            UpdateNavigationPanel();
        }

        private void NavigationPanelPartPressed(BaseNavigationPanelPart panelPressed)
        {
            var CheckClass = panelPressed as VisualClassNavigationPanelPart;
            var checkFunction = panelPressed as VisualFunctionNavigationPanelPart;

            if(CheckClass != null) //Class
            {
                ChangeSelectedEditorIndex(CheckClass.navigationPanelPartIndex);
            }
            else if(checkFunction != null) //Function
            {
                ChangeSelectedEditorIndex(checkFunction.navigationPanelPartIndex);
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

            showingVisualEditors[currentEditorIndex].DisplayAllNodesOnEditor();
        }
    }
}
