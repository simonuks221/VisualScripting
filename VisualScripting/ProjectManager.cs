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

        public ProjectManager(Panel _navigationPanel)
        {
            navigationPanel = _navigationPanel;
            visualProject = new VisualProject();
            visualProject.visualClasses.Add(new VisualClass("Class1"));
            visualProject.visualClasses.Add(new VisualClass("Class2"));

            UpdateNavigationPanel();
        }

        void UpdateNavigationPanel()
        {
            for(int i = 0; i < visualProject.visualClasses.Count; i++)
            {
                Console.Out.WriteLine("a");
                VisualClassNavigationPanelPart newPanel = new VisualClassNavigationPanelPart(visualProject.visualClasses[i]);
                navigationPanel.Controls.Add(newPanel);
                newPanel.Location = new Point(i * 105 + 2, 2);
                newPanel.navigationPanelPressed += NavigationPanelPartPressed;
            }
        }

        private void NavigationPanelPartPressed(BaseNavigationPanelPart panelPressed)
        {
            
        }
    }
}
