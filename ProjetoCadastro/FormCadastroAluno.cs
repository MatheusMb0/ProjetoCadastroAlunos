using ReaLTaiizor.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjetoCadastro
{
    public partial class FormCadastroAluno : ReaLTaiizor.Forms.MaterialForm
    {
        string alunosFileName = "alunos.txt";
        bool isAlteracao = false;
        int indexSelecionado = 0;
        public FormCadastroAluno()
        {
            InitializeComponent();
        }

        private void materialTabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void tabPage2_Click(object sender, EventArgs e)
        {

        }

        private void FormCadastroAluno_Load(object sender, EventArgs e)
        {

        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void airForm1_Click(object sender, EventArgs e)
        {

        }

        private void buttonSalvar_Click(object sender, EventArgs e)
        {
            if (ValidaFormulario()) //Faz a validação
            {
                Salvar(); //chama o método para salvar em arquivo
                TabControlCadastro.SelectedIndex = 1; //muda para a página de consulta
            }
        }

        private bool ValidaFormulario()
        {
            if (string.IsNullOrEmpty(txtMatricula.Text))
            {
                MessageBox.Show("Matrícula obrigatória!");
                txtMatricula.Focus();
                return false;
            }
            if (string.IsNullOrEmpty(txtNome.Text))
            {
                MessageBox.Show("Nome obrigatório!");
                txtNome.Focus();
                return false;
            }
            if (string.IsNullOrEmpty(txtEndereco.Text))
            {
                MessageBox.Show("Endereço obrigatório!");
                txtEndereco.Focus();
                return false;
            }
            if (string.IsNullOrEmpty(txtBairro.Text))
            {
                MessageBox.Show("Bairro obrigatório!");
                txtBairro.Focus();
                return false;
            }
            if (string.IsNullOrEmpty(txtCidade.Text))
            {
                MessageBox.Show("Cidade obrigatória!");
                txtCidade.Focus();
                return false;
            }
            if (string.IsNullOrEmpty(txtSenha.Text))
            {
                MessageBox.Show("Senha obrigatória!");
                txtSenha.Focus();
                return false;
            }
            if (!DateTime.TryParse(txtDataNascimento.Text, out DateTime _))
            {
                MessageBox.Show("Data de Nascimento inválida", "IFSP", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtDataNascimento.Focus();
                return false;
            }
            return true;
        }
        private void Salvar()
        {
            var line = $"{txtMatricula.Text};" +
                       $"{txtDataNascimento.Text};" +
                       $"{txtNome.Text};" +
                       $"{txtEndereco.Text};" +
                       $"{txtBairro.Text};" +
                       $"{txtCidade.Text};" +
                       $"{cboEstado.Text};" +
                       $"{txtSenha.Text};";

            if (!isAlteracao) //novo registro
            {
                var file = new StreamWriter(alunosFileName, true);
                file.WriteLine(line);
                file.Close();
            }
            else //alteração
            {
                string[] alunos = File.ReadAllLines(alunosFileName);
                alunos[indexSelecionado] = line;
                File.WriteAllLines(alunosFileName, alunos);

            }
            LimpaCampos();
        }

        private void LimpaCampos()
        {
            isAlteracao = false;
            foreach (var control in tabPageCadastro.Controls)
            {
                if (control is MaterialTextBoxEdit)
                {
                    ((MaterialTextBoxEdit)control).Clear();
                }
                if (control is MaterialMaskedTextBox)
                {
                    ((MaterialMaskedTextBox)control).Clear();
                }
            }
        }

        private void Carregalistview()
        {
            Cursor.Current = Cursors.WaitCursor;
            mlvAlunos.Columns.Clear();
            mlvAlunos.Items.Clear();
            mlvAlunos.Columns.Add("Matricula");
            mlvAlunos.Columns.Add("Data Nasc");
            mlvAlunos.Columns.Add("Nome");
            mlvAlunos.Columns.Add("Endereço");
            mlvAlunos.Columns.Add("Bairro");
            mlvAlunos.Columns.Add("Cidade");
            mlvAlunos.Columns.Add("UF");

            string[] alunos = File.ReadAllLines(alunosFileName);


            foreach (string aluno in alunos)
            {
                var campos = aluno.Split(';');
                mlvAlunos.Items.Add(new ListViewItem(campos));
            }
            mlvAlunos.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
            Cursor.Current = Cursors.Default;


        }

        private void tabPageConsulta_Enter(object sender, EventArgs e)
        {
            Carregalistview();
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            Editar();
        }
        private void Editar()
        {
            if (mlvAlunos.SelectedItems.Count > 0)
            {
                indexSelecionado = mlvAlunos.SelectedItems[0].Index;
                isAlteracao = true;
                var item = mlvAlunos.SelectedItems[0];
                txtMatricula.Text = item.SubItems[0].Text;
                txtDataNascimento.Text = item.SubItems[1].Text;
                txtNome.Text = item.SubItems[2].Text;
                txtEndereco.Text = item.SubItems[3].Text;
                txtBairro.Text = item.SubItems[4].Text;
                cboEstado.Text = item.SubItems[6].Text;
                txtCidade.Text = item.SubItems[5].Text;
                txtSenha.Text = item.SubItems[7].Text;
                TabControlCadastro.SelectedIndex = 0;
                txtMatricula.Focus();
            }
            else
            {
                MessageBox.Show("selecione algum aluno", "Atenção", MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
            }
        }

        private void mlvAlunos_DoubleClick(object sender, EventArgs e)
        {
            Editar();
        }

        private void buttonCancelar_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(this, "atenção não salvas serão perdidas. \r\n" + "Deseja Cancelar?",
                "pergunta",MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                LimpaCampos();
                TabControlCadastro.SelectedIndex = 1;
            }
        }

        private void btnNovo_Click(object sender, EventArgs e)
        {
            LimpaCampos();
            TabControlCadastro.SelectedIndex = 0;
            txtMatricula.Focus();
        }


        private void excluir()
        {
            List<string> alunos = File.ReadAllLines(alunosFileName).ToList();
            alunos.RemoveAt(indexSelecionado);
            File.WriteAllLines(alunosFileName, alunos);
        }

        private void btnExcluir_Click(object sender, EventArgs e)
        {
            if (mlvAlunos.SelectedIndices.Count > 0) {

                if (MessageBox.Show(this,"deseja realmente deletar o item selecionado?","pergunta ",
                MessageBoxButtons.YesNo,MessageBoxIcon.Question) == DialogResult.Yes) {

                    indexSelecionado = mlvAlunos.SelectedItems[0].Index;
                    excluir();
                    Carregalistview();

                    
                }


            }
            else
            {
                MessageBox.Show("selecione algum aluno", "atenção", MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }

        }
    }
}
