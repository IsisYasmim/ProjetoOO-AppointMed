using System.Text.RegularExpressions;

namespace AppointMed.AppointMed.Core.ValueObjects
{
    public record Email
    {
        public string Valor { get; }

        private Email(string valor)
        {
            if (!IsValid(valor))
                throw new ArgumentException("Email inválido", nameof(valor));

            Valor = valor.Trim().ToLower();
        }

        public static Email Create(string email)
        {
            return new Email(email);
        }

        private static bool IsValid(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                // Regex simples para validação de email
                var regex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
                return regex.IsMatch(email);
            }
            catch
            {
                return false;
            }
        }

        public override string ToString() => Valor;

        // Operadores implícitos para facilitar o uso
        public static implicit operator string(Email email) => email?.Valor;
        public static implicit operator Email(string email) => email == null ? null : Create(email);
    }
}
