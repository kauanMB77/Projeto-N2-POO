﻿using Projeto_N2_POO.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Projeto_N2_POO.Forms
{
    public partial class frGerenciarMoto : frBase
    {
        private List<VeiculoMoto> veiculosDoTipoCadastrados = new List<VeiculoMoto>();
        private int index = 0;
        public frGerenciarMoto()
        {
            InitializeComponent();

            foreach (VeiculoBase veiculo in Dados.Veiculos)
                if (veiculo is VeiculoMoto)
                    veiculosDoTipoCadastrados.Add(veiculo as VeiculoMoto);

            if (veiculosDoTipoCadastrados.Count == 0)
            {
                ExibeTelaDeErro(this);
                return;
            }

            cbmPedagios.DataSource = Dados.Pedagios;
            cbmPedagios.DisplayMember = "Identificacao";

            #region Atribuição de Delegados

            ucAcelerar.metodoAtualizacao = new User_Controls.ucAcelerar.AtualizarValores(AtualizaValor);
            ucDesacelerar.metodoAtualizacao = new User_Controls.ucDesacelerar.AtualizarValores(AtualizaValor);

            #endregion

            AtualizaValor();
        }
        public void AtualizaValor()
        {
            ucIdentificacao.TextoLabel2 = veiculosDoTipoCadastrados[index].Identificacao;
            ucMarca.TextoLabel2 = veiculosDoTipoCadastrados[index].Modelo.Marca.Descricao;
            ucModelo.TextoLabel2 = veiculosDoTipoCadastrados[index].Modelo.Descricao;
            ucVelocidade.TextoLabel2 = veiculosDoTipoCadastrados[index].Velocidade.ToString() + " km/h";

            ucQtdPassageiro.TextoLabel2 = veiculosDoTipoCadastrados[index].CapacidadeDePassageiro.ToString();
            ucEstadoEmpinando.TextoLabel2 = veiculosDoTipoCadastrados[index].Empinando ? "Sim" : "Não";

            ucAcelerar.VeiculoControlado = veiculosDoTipoCadastrados[index];
            ucDesacelerar.VeiculoControlado = veiculosDoTipoCadastrados[index];
        }
        private void btnProximo_Click(object sender, EventArgs e)
        {
            if (index < veiculosDoTipoCadastrados.Count - 1)
                index++;
            else
                MessageBox.Show("Este é a última moto cadastrado.", "Erro !", MessageBoxButtons.OK,
                                 MessageBoxIcon.Error);

            AtualizaValor();
        }
        private void btnAnterior_Click(object sender, EventArgs e)
        {
            if (index > 0)
                index--;
            else
                MessageBox.Show("Este é a primeira moto cadastrado.", "Erro !", MessageBoxButtons.OK,
                                 MessageBoxIcon.Error);

            AtualizaValor();
        }
        private void btnPesquisa_Click(object sender, EventArgs e)
        {
            int aux = veiculosDoTipoCadastrados.FindIndex(c => c.Identificacao == txtPesquisa.Text);

            if (aux >= 0)
                index = aux;
            else
                MessageBox.Show("Esta moto não existe.", "Erro !", MessageBoxButtons.OK,
                                 MessageBoxIcon.Error);

            AtualizaValor();
        }
        private void btnAlterarEmpinando_Click(object sender, EventArgs e)
        {
            veiculosDoTipoCadastrados[index].Empinar();
            Dados.SalvarVeiculos();
            AtualizaValor();
        }
        private void btnPagarPedagio_Click(object sender, EventArgs e)
        {
            if (cbmPedagios.SelectedItem == null)
            {
                MessageBox.Show("Não existe pedágio selecionado", "Erro !", MessageBoxButtons.OK,
                                 MessageBoxIcon.Error);
                return;
            }

            MessageBox.Show((cbmPedagios.SelectedItem as Pedagio).Receber(veiculosDoTipoCadastrados[index]), "Sucesso !", MessageBoxButtons.OK,
                                 MessageBoxIcon.Information);
            Dados.SalvarPedagios();
        }
    }
}
