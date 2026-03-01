using SSMSObjectExplorerMenu;
using static SSMSObjectExplorerMenu.extendedfiltering.ExtendedFiltering;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace SSMSObjectExplorer.controls
{
    public partial class AdvancedFilterControl : UserControl
    {
        private ComboBox _contextComboBox = null;
        private string _currentContext;
        private readonly Dictionary<string, (bool Server, bool Database, bool Table, bool Column)> _contextFields = new Dictionary<string, (bool, bool, bool, bool)>
        {
            { Constants.Server_Context, (true, false, false, false) },
            { Constants.Database_Context, (true, true, false, false) },
            { Constants.Table_Context, (true, true, true, false) },
            { Constants.Column_Context, (true, true, true, true) }
        };

        public ComboBox ContextComboBox
        {
            get => _contextComboBox;
            set
            {
                if(value != null)
                {
                    _contextComboBox = value;
                    _currentContext = _contextComboBox.SelectedValue as string;

                    HandleMenuItemContextChange(_contextComboBox, EventArgs.Empty);
                    _contextComboBox.SelectedValueChanged += HandleMenuItemContextChange;
                }
            }
        }

        public string Filter
        {
            get
            {
                switch(_currentContext)
                {
                    case Constants.Server_Context:
                        return ServerFilter(serverTextBox.Text);
                    case Constants.Database_Context:
                        return DatabaseFilter(databaseTextBox.Text, serverTextBox.Text);
                    case Constants.Table_Context:
                        return TableFilter(tableTextBox.Text, schemaTextBox.Text, databaseTextBox.Text, serverTextBox.Text);
                    case Constants.Column_Context:
                        return ColumnFilter(columnTextBox.Text, tableTextBox.Text, schemaTextBox.Text, databaseTextBox.Text, serverTextBox.Text);
                    default:
                        return string.Empty;
                }
            }
        }

        public AdvancedFilterControl()
        {
            InitializeComponent();
        }

        public void HandleMenuItemContextChange(object sender, EventArgs e)
        {
            if(sender is ComboBox contextComboBox)
            {
                var selectedContext = contextComboBox.SelectedItem as string;
                // Selected context in the combo box has changed or the dialog is initializing
                if(selectedContext != _currentContext || selectedContext is null)
                {
                    ChangeControlStates(selectedContext);
                    _currentContext = selectedContext;
                    return;
                }
            }
        }

        private void ChangeControlStates(string selectedContext)
        {
            if(selectedContext != null && _contextFields.TryGetValue(selectedContext, out var states))
            {
                ChangeControlStates(states.Server, states.Database, states.Table, states.Column);
                return;
            }
            ChangeControlStates(false, false, false, false);
        }

        private void ChangeControlStates(bool server, bool database, bool table, bool column)
        {
            ChangeControlState(serverTextBox, serverLabel, server);
            ChangeControlState(databaseTextBox, databaseLabel, database);
            ChangeControlState(schemaTextBox, schemaLabel, table);
            ChangeControlState(tableTextBox, tableLabel, table);
            ChangeControlState(columnTextBox, columnLabel, column);
        }

        private void ChangeControlState(TextBox tb, Label label, bool enabled)
        {
            bool changedEnabledState = tb.Enabled != enabled;
            bool enabling = enabled && !tb.Enabled;
            var newText = changedEnabledState
                ? (enabling ? Wildcard_Any : string.Empty)
                // Text box remains disabled
                // Text box remains enaled, but user entered only whitespace characters
                : (!tb.Enabled || !string.IsNullOrWhiteSpace(tb.Text) ? tb.Text : Wildcard_Any);

            tb.Enabled = enabled;
            tb.Text = newText;
            label.Enabled = enabled;
        }
    }
}
