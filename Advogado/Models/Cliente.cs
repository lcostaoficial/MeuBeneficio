using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Script.Serialization;

namespace Advogado.Models
{
    public class Cliente
    {
        public int ClienteId { get; set; }

        [Display(Name = "Nome")]
        [MinLength(1, ErrorMessage = "Mínimo 1 caractere")]
        [MaxLength(255, ErrorMessage = "Máximo 255 caracteres")]
        [Required(ErrorMessage = "Campo obrigatório")]
        public string Nome { get; set; }

        [Display(Name = "Nome da mãe")]
        [MinLength(1, ErrorMessage = "Mínimo 1 caractere")]
        [MaxLength(255, ErrorMessage = "Máximo 255 caracteres")]
        [Required(ErrorMessage = "Campo obrigatório")]
        public string NomeMae { get; set; }

        [Display(Name = "CEP")]
        [MinLength(1, ErrorMessage = "Mínimo 1 caractere")]
        [MaxLength(10, ErrorMessage = "Máximo 10 caracteres")]
        [Required(ErrorMessage = "Campo obrigatório")]
        public string Cep { get; set; }

        [Display(Name = "Logradouro")]
        [MinLength(1, ErrorMessage = "Mínimo 1 caractere")]
        [MaxLength(255, ErrorMessage = "Máximo 255 caracteres")]
        [Required(ErrorMessage = "Campo obrigatório")]
        public string Rua { get; set; }

        [Display(Name = "Número")]
        [MinLength(1, ErrorMessage = "Mínimo 1 caractere")]
        [MaxLength(10, ErrorMessage = "Máximo 255 caracteres")]
        [Required(ErrorMessage = "Campo obrigatório")]
        public string Numero { get; set; }

        [Display(Name = "Bairro")]
        [MinLength(1, ErrorMessage = "Mínimo 1 caractere")]
        [MaxLength(255, ErrorMessage = "Máximo 255 caracteres")]
        [Required(ErrorMessage = "Campo obrigatório")]
        public string Bairro { get; set; }

        [Display(Name = "Cidade")]
        [MinLength(1, ErrorMessage = "Mínimo 1 caractere")]
        [MaxLength(255, ErrorMessage = "Máximo 255 caracteres")]
        [Required(ErrorMessage = "Campo obrigatório")]
        public string Cidade { get; set; }
       
        [Display(Name = "Unidade federal")]
        [Required(ErrorMessage = "Campo obrigatório")]
        [Range(1, int.MaxValue, ErrorMessage = "Campo obrigatório")]
        [EnumDataType(typeof(Estado), ErrorMessage = "Campo obrigatório")]
        public Estado Estado { get; set; }

        [Display(Name = "Naturalidade")]
        [MinLength(1, ErrorMessage = "Mínimo 1 caractere")]
        [MaxLength(100, ErrorMessage = "Máximo 100 caracteres")]
        [Required(ErrorMessage = "Campo obrigatório")]
        public string Naturalidade { get; set; }

        [Display(Name = "Nacionalidade")]
        [MinLength(1, ErrorMessage = "Mínimo 1 caractere")]
        [MaxLength(100, ErrorMessage = "Máximo 100 caracteres")]
        [Required(ErrorMessage = "Campo obrigatório")]
        public string Nacionalidade { get; set; }

        [Display(Name = "Data de nascimento")]
        [Required(ErrorMessage = "Campo obrigatório")]
        public DateTime DataNascimento { get; set; }

        [Display(Name = "UF de nascimento")]
        [Required(ErrorMessage = "Campo obrigatório")]
        [Range(1, int.MaxValue, ErrorMessage = "Campo obrigatório")]
        [EnumDataType(typeof(Estado), ErrorMessage = "Campo obrigatório")]
        public Estado EstadoNascimento { get; set; }

        [Display(Name = "Profissão")]
        [MinLength(1, ErrorMessage = "Mínimo 1 caractere")]
        [MaxLength(100, ErrorMessage = "Máximo 100 caracteres")]
        [Required(ErrorMessage = "Campo obrigatório")]
        public string Profissao { get; set; }

        [Display(Name = "RG")]
        //[MinLength(1, ErrorMessage = "Mínimo 1 caractere")]
        //[MaxLength(20, ErrorMessage = "Máximo 20 caracteres")]
        //[Required(ErrorMessage = "Campo obrigatório")]
        public string Rg { get; set; }
        
        [Display(Name = "Orgão expedidor do RG")]
        //[Required(ErrorMessage = "Campo obrigatório")]
        //[Range(1, int.MaxValue, ErrorMessage = "Campo obrigatório")]
        //[EnumDataType(typeof(OrgaoExpedidorRg), ErrorMessage = "Campo obrigatório")]
        public OrgaoExpedidorRg OrgaoExpedidorRg { get; set; }

        [Display(Name = "Estado emissor do RG")]
        //[Required(ErrorMessage = "Campo obrigatório")]
        //[Range(1, int.MaxValue, ErrorMessage = "Campo obrigatório")]
        //[EnumDataType(typeof(Estado), ErrorMessage = "Campo obrigatório")]
        public Estado EstadoOrgaoExpedidor { get; set; }

        [Display(Name = "CPF")]
        [MinLength(1, ErrorMessage = "Mínimo 1 caractere")]
        [MaxLength(14, ErrorMessage = "Máximo 14 caracteres")]
        [Required(ErrorMessage = "Campo obrigatório")]
        public string Cpf { get; set; }

        [Display(Name = "Telefone móvel")]
        [MinLength(15, ErrorMessage = "Mínimo 15 caractere")]
        [MaxLength(15, ErrorMessage = "Máximo 15 caracteres")]
        [Required(ErrorMessage = "Campo obrigatório")]
        public string TelefoneMovel { get; set; }

        [Display(Name = "Telefone fixo")]
        [MinLength(14, ErrorMessage = "Mínimo 14 caractere")]
        [MaxLength(14, ErrorMessage = "Máximo 14 caracteres")]
        public string TelefoneFixo { get; set; }

        [Display(Name = "E-mail")]
        [MinLength(1, ErrorMessage = "Mínimo 1 caractere")]
        [MaxLength(255, ErrorMessage = "Máximo 255 caracteres")]
        [Required(ErrorMessage = "Campo obrigatório")]
        public string Email { get; set; }
       
        [Display(Name = "Estado civil")]
        [Required(ErrorMessage = "Campo obrigatório")]
        [Range(1, int.MaxValue, ErrorMessage = "Campo obrigatório")]
        [EnumDataType(typeof(EstadoCivil), ErrorMessage = "Campo obrigatório")]
        public EstadoCivil EstadoCivil { get; set; }

        [Display(Name = "Cliente estrangeiro?")]       
        [Required(ErrorMessage = "Campo obrigatório")]
        public bool ClienteEstrangeiro { get; set; }

        [Display(Name = "Documento do estrangeiro")]
        public string DocumentoIdentificacaoEstrangeiro { get; set; }

        public string EstadoCivilFormatado => EstadoCivil.ToString();
        public string EstadoFormatado => Estado.ToString();

        public string OrgaoExpedidorRgFormatado => OrgaoExpedidorRg.ToString();
        public string EstadoOrgaoExpedidorFormatado => EstadoOrgaoExpedidor.ToString();

        public string DataNascimentoFormatado => DataNascimento.ToString("dd/MM/yyyy");

        public string PortadorDocumento => ClienteEstrangeiro ? $"portador do documento de identificação {DocumentoIdentificacaoEstrangeiro}" : $"portador do RG {Rg} {OrgaoExpedidorRgFormatado}/{EstadoOrgaoExpedidor}";

        [ScriptIgnore]
        public Collection<Caso> Casos { get; set; }

        [ScriptIgnore]
        public Collection<Caso> CasosRepresentantesLegais { get; set; }

        public void Atualizar(Cliente model)
        {
            Nome = model.Nome;
            NomeMae = model.NomeMae;
            Rua = model.Rua;
            Numero = model.Numero;
            Bairro = model.Bairro;
            Cidade = model.Cidade;
            Estado = model.Estado;
            Naturalidade = model.Naturalidade;
            Nacionalidade = model.Nacionalidade;
            DataNascimento = model.DataNascimento;
            Profissao = model.Profissao;
            ClienteEstrangeiro = model.ClienteEstrangeiro;           
            TelefoneMovel = model.TelefoneMovel;
            TelefoneFixo = model.TelefoneFixo;
            Email = model.Email;
            EstadoCivil = model.EstadoCivil;
            SetEstrangeiro(model);
        }

        public void SetEstrangeiro(Cliente model)
        {
            if (model.ClienteEstrangeiro == false)
            {
                Rg = model.Rg;
                OrgaoExpedidorRg = model.OrgaoExpedidorRg;
                EstadoOrgaoExpedidor = model.EstadoOrgaoExpedidor;
                DocumentoIdentificacaoEstrangeiro = "";
            }
            else
            {
                Rg = string.Empty;
                OrgaoExpedidorRg = 0;
                EstadoOrgaoExpedidor = 0;
                DocumentoIdentificacaoEstrangeiro = model.DocumentoIdentificacaoEstrangeiro;
            }            
        }

        public bool ValidarCpf()
        {
            int[] multiplicador1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            string tempCpf;
            string digito;
            int soma;
            int resto;
            var cpf = Cpf.Trim();
            cpf = cpf.Replace(".", "").Replace("-", "");
            if (cpf.Length != 11)
                return false;
            tempCpf = cpf.Substring(0, 9);
            soma = 0;

            for (int i = 0; i < 9; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];
            resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito = resto.ToString();
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
            return cpf.EndsWith(digito);
        }

    }
}