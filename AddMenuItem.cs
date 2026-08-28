using SSMSObjectExplorerMenu.extendedfiltering;
using SSMSObjectExplorerMenu.extensions;
using SSMSObjectExplorerMenu.objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using MenuItem = SSMSObjectExplorerMenu.objects.MenuItem;

namespace SSMSObjectExplorerMenu
{
	public partial class AddMenuItem : Form
	{		
		public AddMenuItem(NodeInfo nodeInfo)
		{
			InitializeComponent();

			labelVersion.Text = Assembly.GetExecutingAssembly().GetName().Version.ToString();
			comboContext.Text = nodeInfo.UrnPath;
            this.ActiveControl = textName;
			buttonOpen.Visible = false;

			listViewUserDefinedParam.Columns[0].Width = -2;
			listViewUserDefinedParam.Columns[1].Width = -2;
			listViewUserDefinedParam.Columns[2].Width = -2;
		}

		public MenuItem GetMenuItem()
		{
			return new MenuItem(true, comboContext.Text, textName.Text, textPath.Text, checkExecute.Checked, checkConfirm.Checked, advancedFilterControl.Filter, listViewUserDefinedParam.GetUserDefinedParams());
		}
		
		private void textName_TextChanged(object sender, EventArgs e)
		{
			//ValidateInputs();
		}

		private void textPath_TextChanged(object sender, EventArgs e)
		{
			//ValidateInputs();
		}

		private bool TryValidateInputs(out IEnumerable<string> validationErrors)
		{
			validationErrors = [];

            if (textName.Text.Trim().Length == 0)
                validationErrors.Append("Name cannot be empty.");
            if (textPath.Text.Trim().Length == 0)
                validationErrors.Append("Path cannot be empty.");

            var filter = ExtendedFilteringProperties.BuildFromNavigationContext($"{this.advancedFilterControl.Filter}", out var buildFilterErrors);
			if (filter != null && !filter.TryValidateForContext(comboContext.Text, out var contextErrors))
                validationErrors.Concat(contextErrors);
            else if (filter == null)
                validationErrors.Concat(buildFilterErrors);

			return !validationErrors.Any();
		}

		private void buttonOpen_Click(object sender, EventArgs e)
		{
			if (openFileDialog.ShowDialog() == DialogResult.OK)
			{ 
				textPath.Text = openFileDialog.FileName;
			}
		}

		private void radioScript_CheckedChanged(object sender, EventArgs e)
		{
			updateDisplay();
		}

		private void radioPath_CheckedChanged(object sender, EventArgs e)
		{
			updateDisplay();
		}

		private void updateDisplay()
		{
			textPath.Text = string.Empty;
			buttonOpen.Visible = radioPath.Checked;
		}

		private void checkConfirm_CheckedChanged(object sender, EventArgs e)
		{
			if (checkConfirm.Checked)
			{ 
				checkExecute.Checked = true;
			}
		}

		private void checkExecute_CheckedChanged(object sender, EventArgs e)
		{
			if (!checkExecute.Checked)
			{
				checkConfirm.Checked = false;
			}
		}

        private void buttonAddUserDefinedParam_Click(object sender, EventArgs e)
        {
            var addDialog = new AddUserDefinedParameter(GetArgumentNamesInUse());
            if (addDialog.ShowDialog() == DialogResult.OK)
            {
                var newParam = addDialog.Parameter;
                var newListViewItem = new ListViewItem { Text = "{" + newParam.Name +"}", Tag = newParam };
                newListViewItem.SubItems.Add(new ListViewItem.ListViewSubItem { Text = newParam.Type.ToStringDescription() });
				newListViewItem.SubItems.Add(new ListViewItem.ListViewSubItem { Text = newParam.DefaultValueAsString });

				listViewUserDefinedParam.Items.Add(newListViewItem);
			}
		}

        private void buttonEditUserDefinedParam_Click(object sender, EventArgs e)
        {
			var selectedItem = this.listViewUserDefinedParam.GetSelectedItems().SingleOrDefault();
            if (selectedItem != null)
            {
				var editDialog = new AddUserDefinedParameter(GetArgumentNamesInUse(), true, (UserDefinedParameter)selectedItem.Tag);
				if (editDialog.ShowDialog() == DialogResult.OK)
				{
					var editedParam = editDialog.Parameter;
					selectedItem.Text = "{" + editedParam.Name + "}";
					selectedItem.SubItems[1].Text = editedParam.Type.ToStringDescription();
					selectedItem.SubItems[2].Text = editedParam.DefaultValueAsString;
					selectedItem.Tag = editedParam;
                }
            }
		}

        private void buttonRemoveUserDefinedParam_Click(object sender, EventArgs e)
        {
			var selectedItems = this.listViewUserDefinedParam.GetSelectedItems();
            if (selectedItems.Any() &&
                DialogResult.Yes == MessageBox.Show("Are you sure?", "Delete parameter(s)", MessageBoxButtons.YesNo, MessageBoxIcon.Warning))
            {
                foreach (var item in selectedItems)
                {
                    this.listViewUserDefinedParam.Items.Remove(item);
                }
            }
		}

        private void listViewUserDefinedParam_SelectedIndexChanged(object sender, EventArgs e)
        {
			// In case of removing items, a minimal delay is needed to work with the state of the listview after the item has been removed.
            this.BeginInvoke(() =>
			{
				var selectedItemsCount = this.listViewUserDefinedParam.GetSelectedItems().Count();
				this.buttonEditUserDefinedParameter.Enabled = selectedItemsCount == 1;
				this.buttonRemoveUserDefinedParam.Enabled = selectedItemsCount > 0;
			});
        }

        private IEnumerable<string> GetArgumentNamesInUse() => this.listViewUserDefinedParam.Items.Cast<ListViewItem>().Select(item => item.Text).Concat(Utils.ParametersFromContext);

		private void buttonOK_Click(object sender, EventArgs e)
		{
			if (TryValidateInputs(out var errors))
			{
				this.DialogResult = DialogResult.OK;
				this.Close();
			}
			else
			{
				var dlgTextErrors = string.Join(Environment.NewLine, errors);
                MessageBox.Show($"One- or more error(s) occurred:{Environment.NewLine}{dlgTextErrors}", "Menu item cannot be saved", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
    }
}
