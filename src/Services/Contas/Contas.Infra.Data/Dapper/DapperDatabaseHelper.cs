using Dapper;
using System.Data;

namespace Contas.Infra.Data.Dapper
{
    public class DapperDatabaseHelper
    {
        public static void CreateTablesIfNotExists(IDbConnection conneciton)
        {
            var sql = CreateTableCliente();
            sql = string.Join('\n', sql, CreateTableConta());
            sql = string.Join('\n', sql, CreateTableCartao());

            conneciton.Open();
            conneciton.Execute(sql);
            conneciton.Close();
        }

        private static string CreateTableCliente()
        {
            var sql = @"IF (NOT EXISTS (SELECT *FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_NAME = 'Cliente'))
                BEGIN
                    CREATE TABLE [dbo].[Cliente](
                        [Id] [bigint] NOT NULL,
                        [CPF] [nvarchar](12) NOT NULL,
                        [Nome] [nvarchar](70) NOT NULL
                        CONSTRAINT [PK_Cliente] PRIMARY KEY CLUSTERED 
                        (
                            [Id] ASC
                        )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
                    ) ON [PRIMARY]
                END";

            return sql;
        }
        private static string CreateTableConta()
        {
            var sql = @"IF (NOT EXISTS (SELECT *FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_NAME = 'Conta'))
                BEGIN
                    CREATE TABLE [dbo].[Conta](
                        [Id] [bigint] IDENTITY(1,1) NOT NULL,
                        [ClienteId] [bigint] NOT NULL FOREIGN KEY REFERENCES Cliente([Id]),
                        [Saldo] [decimal](10,2) NOT NULL
                        CONSTRAINT [PK_Conta] PRIMARY KEY CLUSTERED 
                        (
                            [Id] ASC
                        )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
                    ) ON [PRIMARY]
                END";

            return sql;
        }
        private static string CreateTableCartao()
        {
            var sql = @"IF (NOT EXISTS (SELECT *FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_NAME = 'Cartao'))
                BEGIN
                    CREATE TABLE [dbo].[Cartao](
                        [Id] [bigint] IDENTITY(1,1) NOT NULL,
                        [ContaId] [bigint] NOT NULL FOREIGN KEY REFERENCES Conta([Id]),
                        [Numero] [nvarchar](17) NOT NULL,
                        [Codigo] [nvarchar](4) NOT NULL,
                        [Bloqueado] [bit] NOT NULL DEFAULT(0)
                        CONSTRAINT [PK_Cartao] PRIMARY KEY CLUSTERED 
                        (
                            [Id] ASC
                        )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
                    ) ON [PRIMARY]
                END";

            return sql;
        }
    }
}