-- Criar a base de dados
CREATE DATABASE IF NOT EXISTS projeto_faturacao
CHARACTER SET utf8mb4 
COLLATE utf8mb4_unicode_ci;

USE projeto_faturacao;

-- Tabela de Clientes
CREATE TABLE IF NOT EXISTS Clientes (
    ID INT AUTO_INCREMENT PRIMARY KEY,
    Nome VARCHAR(150) NOT NULL,
    NIF CHAR(9) NOT NULL UNIQUE,
    Contato VARCHAR(20),
    Email VARCHAR(100),
    Morada VARCHAR(255),
    Localidade VARCHAR(100),
    Codigo_Postal VARCHAR(10)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Tabela de Fornecedores
CREATE TABLE IF NOT EXISTS Fornecedores (
    ID INT AUTO_INCREMENT PRIMARY KEY,
    Nome_Empresa VARCHAR(150) NOT NULL,
    NIPC CHAR(9) NOT NULL UNIQUE,
    Nome_Representante VARCHAR(150),
    Contato VARCHAR(20),
    Email VARCHAR(100),
    Rua VARCHAR(255),
    Localidade VARCHAR(100),
    Codigo_Postal VARCHAR(10)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Tabela de Produtos
CREATE TABLE IF NOT EXISTS Produtos (
    ID INT AUTO_INCREMENT PRIMARY KEY,
    Cod_Fornecedor INT NOT NULL,
    Nome VARCHAR(150) NOT NULL,
    Referencia VARCHAR(50) UNIQUE,
    Descricao TEXT,
    Preco_Custo DECIMAL(10, 2) NOT NULL DEFAULT 0.00,
    Preco_Venda DECIMAL(10, 2) NOT NULL DEFAULT 0.00,
    Unidade_Medida VARCHAR(20),
    IVA DECIMAL(5, 2) NOT NULL DEFAULT 0.00,
    Stock_Atual DECIMAL(10, 2) NOT NULL DEFAULT 0.00,
    FOREIGN KEY (Cod_Fornecedor) REFERENCES Fornecedores(ID) ON DELETE RESTRICT
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Tabela de Faturas (Cabeçalho)
CREATE TABLE IF NOT EXISTS Faturas (
    Id_Fatura INT AUTO_INCREMENT PRIMARY KEY,
    Id_Cliente INT NOT NULL,
    Numero_Fatura VARCHAR(50) NOT NULL UNIQUE,
    Data_Emissao DATETIME NOT NULL,
    Valor_Total_Liquido DECIMAL(10, 2) NOT NULL,
    Valor_Total_IVA DECIMAL(10, 2) NOT NULL,
    Valor_Total_Pagar DECIMAL(10, 2) NOT NULL,
    Estado VARCHAR(20) DEFAULT 'Emitida',
    FOREIGN KEY (Id_Cliente) REFERENCES Clientes(ID) ON DELETE RESTRICT
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Tabela de Linhas da Fatura (Detalhes)
CREATE TABLE IF NOT EXISTS Linhas_fatura (
    Id_Linha INT AUTO_INCREMENT PRIMARY KEY,
    Id_Fatura INT NOT NULL,
    id_Produto INT NOT NULL,
    Quantidade INT NOT NULL,
    Preco_Unitario DECIMAL(10, 2) NOT NULL,
    taxa_iva DECIMAL(5, 2) NOT NULL,
    subtotal DECIMAL(10, 2) NOT NULL,
    FOREIGN KEY (Id_Fatura) REFERENCES Faturas(Id_Fatura) ON DELETE CASCADE,
    FOREIGN KEY (id_Produto) REFERENCES Produtos(ID) ON DELETE RESTRICT
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Índices para melhorar a velocidade de pesquisa
CREATE INDEX idx_nif_cliente ON Clientes(NIF);
CREATE INDEX idx_nipc_fornecedor ON Fornecedores(NIPC);
CREATE INDEX idx_ref_produto ON Produtos(Referencia);