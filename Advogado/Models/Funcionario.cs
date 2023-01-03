using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Script.Serialization;

namespace Advogado.Models
{
    public class Funcionario
    {
        public int FuncionarioId { get; set; }

        [Display(Name = "Nome")]
        [MinLength(1, ErrorMessage = "Mínimo 1 caractere")]
        [MaxLength(255, ErrorMessage = "Máximo 255 caracteres")]
        [Required(ErrorMessage = "Campo obrigatório")]
        public string Nome { get; set; }

        [Display(Name = "CPF")]
        [MinLength(1, ErrorMessage = "Mínimo 1 caractere")]
        [MaxLength(14, ErrorMessage = "Máximo 14 caracteres")]
        [Required(ErrorMessage = "Campo obrigatório")]
        public string Cpf { get; set; }

        public string Senha { get; set; }

        public Perfil Perfil { get; set; }

        public bool Ativo { get; set; }

        public bool ValidarUsuarioSenha(string hashCorreto)
        {
            var hashsenhaDigitada = GerarMd5(Senha);
            if (hashCorreto == hashsenhaDigitada) return true;
            return false;
        }

        public string GerarMd5(string senha)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.ASCII.GetBytes(senha);
                byte[] hashBytes = md5.ComputeHash(inputBytes);
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }

        public bool ValidarCpf()
        {
            int[] multiplicador1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            var cpfFormatado = Cpf.Trim().Replace(".", "").Replace("-", "");
            if (cpfFormatado.Length != 11)
                return false;
            for (int j = 0; j < 10; j++)
                if (j.ToString().PadLeft(11, char.Parse(j.ToString())) == cpfFormatado)
                    return false;
            string tempCpf = cpfFormatado.Substring(0, 9);
            int soma = 0;
            for (int i = 0; i < 9; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];
            int resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            string digito = resto.ToString();
            tempCpf = tempCpf + digito;
            soma = 0;
            for (int i = 0; i < 10; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];
            resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito = digito + resto.ToString();
            return cpfFormatado.EndsWith(digito);
        }

        public void SenhaPadrao()
        {
            if (string.IsNullOrEmpty(Cpf)) throw new Exception("Não foi possível gerar uma senha padrão para este usuário!");
            var cpfSomenteNumeros = Regex.Replace(Cpf, "[^0-9]", "");
            Senha = GerarMd5(cpfSomenteNumeros);
        }

        public void Atualizar(Funcionario model)
        {
            Nome = model.Nome;
            Perfil = model.Perfil;
        }

        public void InverterAtivo()
        {
            Ativo = !Ativo;
        }

        public static Funcionario FuncionarioLogado()
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<Funcionario>(HttpContext.Current.User.Identity.Name);           
        }

        public Collection<Movimentacao> Movimentacoes { get; set; }

        [ScriptIgnore]
        public Collection<Tarefa> TarefasCriadas { get; set; }

        [ScriptIgnore]
        public Collection<Tarefa> TarefasResponsaveis { get; set; }

        public Collection<JustificativaTarefa> JustificativasTarefas { get; set; }
    }
}