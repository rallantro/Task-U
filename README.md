# Task-u

**Task-u** é um RPG de console (*CLI RPG*) desenvolvido em C# com .NET 10, que aplica mecânicas de gamificação à produtividade. O jogo converte a conclusão de tarefas diárias em progresso dentro de um sistema de RPG, utilizando elementos como gacha, combate por turnos e gerenciamento de inventário.

---

## Sobre o Projeto

O Task-u foi concebido como um motor de gamificação que processa a conclusão de tarefas e as traduz em recompensas no jogo. Cada tarefa finalizada gera Cristais, a moeda interna, que podem ser utilizados no subsistema de invocação (*gacha*). A lógica por trás do gacha inclui:

- **Sistema de Pity:** Contadores internos que garantem a obtenção de personagens de alta raridade após um número definido de tentativas, equilibrando a progressão.
- **Banners Rotativos:** Atualização semanal dos personagens com taxa de aparição aumentada, controlada por lógica de tempo e persistência.
- **Persistência de Estado:** Todo o progresso (inventário, personagens desbloqueados, tarefas concluídas) é armazenado localmente com Entity Framework Core e SQLite.

O projeto demonstra competências em:

- C# e .NET 10
- Entity Framework Core (Code-First, Migrations)
- Programação Orientada a Objetos (herança, polimorfismo, encapsulamento)
- Lógica de jogos (combate por turnos, sistema de probabilidade)
- Arquitetura em camadas (separação entre dados, serviços e apresentação)

---

## Funcionalidades

- **Tarefas:** Geração diária de tarefas principais e side quests; conclusão concede Cristais e pode ativar eventos de sorte.
- **Gacha:** Sistema com raridades (Comum, Raro, Épico, Lendário), pity (soft/hard) e banners rotativos.
- **Combate:** Batalhas por turnos contra inimigos gerados dinamicamente, com habilidades especiais, status (stun, silêncio) e uso de itens consumíveis.
- **Inventário:** Gerenciamento de personagens e itens, com dois slots de equipamento que afetam atributos de combate.
- **Persistência:** Dados salvos em SQLite via EF Core, garantindo continuidade entre sessões.

---

## Como Executar

### Pré-requisitos

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- Entity Framework 
> (Nota: Caso não tenha a ferramenta instalada, execute: dotnet tool install --global dotnet-ef)
- Git (opcional)

### Passos

1. Clone o repositório:
   ```bash
   git clone https://github.com/rallantro/Todo-Gacha.git
   cd Task-u
   ```

2. Restaure as dependências e compile:
   ```bash
   dotnet restore
   dotnet build
   ```

3. Aplique as migrações do banco de dados:
   ```bash
   dotnet ef database update
   ```

4. Execute o jogo:
   ```bash
   dotnet run
   ```

---

## Estrutura do Projeto

```
Task-u/
├── Core/               # Classes base (Personagem, Inimigo, Combate)
├── Data/               # AppDbContext e configurações EF Core
├── Models/             # Entidades do banco de dados (Tarefa, User, etc.)
├── Services/           # Lógica de negócio (Gacha, Inventário, Tarefas)
├── Migrations/         # Migrações geradas pelo EF Core
├── Program.cs          # Loop principal e interação com o usuário
└── gacha_database.db   # Banco de dados SQLite (gerado na primeira execução)
```

## Banco de Dados

O Task-u utiliza um banco de dados SQLite local (`gacha_database.db`) gerado automaticamente na primeira execução. A estrutura é gerenciada pelo Entity Framework Core por meio de migrações, mas você pode inspecionar ou adicionar dados manualmente, se desejar.

### Localização do Arquivo

- O arquivo `gacha_database.db` é criado no diretório raiz do projeto.
- Ele contém todas as informações de usuários, inventário, tarefas, side quests, personagens, itens e inimigos base. Se desejar criar mais fica a seu dispor. 

### Ferramentas Recomendadas

- **DB Browser for SQLite** – Interface gráfica gratuita para visualizar e editar o banco.
- **SQLite CLI** – Linha de comando (`sqlite3 gacha_database.db`).

### Principais Tabelas e Seus Papéis

| Tabela           | Descrição |
|------------------|-----------|
| `Users`          | Dados do jogador: cristais, pity, equipamentos, inimigo atual. |
| `BaseTarefas`    | Modelo de tarefas que serão geradas diariamente. |
| `SideQuests`     | Missões secundárias que aparecem aleatoriamente a cada dia. |
| `Tarefas`        | Tarefas ativas do dia (criadas a partir das tabelas acima). |
| `Personagens`    | Todos os personagens disponíveis no jogo (heróis). |
| `Itens`          | Itens consumíveis e equipáveis. |
| `Inimigos`       | Inimigos que podem ser enfrentados. |

### Como Adicionar Novas Tarefas ou Side Quests

Para incluir novas tarefas diárias ou side quests, você pode inserir registros diretamente nas tabelas `BaseTarefas` e `SideQuests`. Exemplo de inserção via SQL:

```sql
-- Inserir uma nova tarefa diária (DiaSemana: 0 = Domingo, 1 = Segunda, ..., 6 = Sábado)
INSERT INTO BaseTarefas (DiaSemana, Name, Desc, Dif, IsDone)
VALUES (1, 'Estudar SQL', 'Revise os fundamentos de SQL por 30 minutos.', 2, 0);

-- Inserir uma nova side quest
INSERT INTO SideQuests (Name, Desc, Dif, IsDone)
VALUES ('Meditar', 'Faça 10 minutos de meditação.', 1, 0);
```

> **Nota:** Após adicionar novos registros, o jogo os utilizará automaticamente na próxima regeneração diária de tarefas (quando `lastLogin` for atualizado). Não é necessário recriar o banco.

### Observações sobre os Dados Existentes

- O banco já contém um usuário padrão com ID = 1, alguns personagens, itens e inimigos.
- Se você desejar reiniciar o progresso, basta excluir o arquivo `gacha_database.db` e executar `dotnet ef database update` novamente. Isso criará um novo banco com os dados iniciais definidos nas migrações e nos seeders (se houver).
- A quantidade de cristais dada por tarefa é equivalente a dificuldade da tarefa vezes 2.

---

## Arquitetura

O Task-u foi estruturado em camadas para separar responsabilidades e facilitar a manutenção. A organização do código reflete a divisão entre lógica de domínio, serviços de negócio, persistência e interface com o usuário.

```
Task-u/
├── Core/               # Entidades de domínio e lógica central
├── Models/             # Entidades de persistência (EF Core)
├── Data/               # Contexto do banco de dados e configurações
├── Services/           # Serviços de orquestração (regras de negócio)
├── Migrations/         # Migrações geradas pelo EF Core
├── Program.cs          # Loop principal e interação com o usuário
```

### Camadas e Responsabilidades

**Core**  
Contém as classes base que definem o comportamento do jogo, independente de persistência ou interface:
- `PersonagemBase` e suas especializações (ex.: `Barbaro`, `Moon`) – implementam lógica de habilidades, passivas, cálculo de dano.
- `InimigoBase` e suas variações – comportamento específico dos inimigos.
- `CombateEngine`, `TurnoJogador`, `TurnoInimigo` e `CombateUI` – gerenciam o fluxo de batalha, encapsulando a lógica de turnos e a exibição.

**Models**  
Define as entidades que são mapeadas para o banco de dados via Entity Framework Core:
- `User`, `Tarefa`, `BaseTarefas`, `SideQuest`, `Banner`, `Item`, `PersonagemInventario`, `ItemInventario`.
- Essas classes são “puras” (POCOs) e não contêm comportamento de negócio.

**Data**  
Contém `AppDbContext`, que configura o mapeamento entre as entidades e o banco SQLite. É o ponto único de acesso ao banco.

**Services**  
Orquestram a lógica de negócio e coordenam as interações entre as camadas:
- `TarefaService` – gerencia a regeneração diária, conclusão e recompensas.
- `Gacha` – implementa o sorteio, pity e integração com o `BannerService`.
- `BannerService` – controla a rotação semanal e os rate-ups.
- `InventarioServices` – gerencia a visualização e equipamento de personagens e itens.
- `AdventureService` – define o inimigo atual e sua regeneração.
- `CombatService` – atua como fachada para o `CombateEngine`.

**Program.cs**  
Contém o loop principal, o menu e a interação com o usuário. Instancia os serviços e injeta as dependências manualmente (padrão *Poor Man's DI*).

### Fluxo de Dados

1. **Entrada do usuário** → `Program.cs` captura a opção do menu.
2. **Serviço correspondente** é chamado (ex.: `TarefaService.ConcluirTarefa`).
3. O serviço acessa o `AppDbContext` para recuperar/atualizar dados.
4. A lógica de negócio é executada (ex.: calcular cristais, atualizar pity).
5. As alterações são persistidas via `SaveChanges()`.
6. O resultado é exibido no console.

### Padrões Utilizados

- **Herança e Polimorfismo** – usado extensivamente em `PersonagemBase` e `InimigoBase` para permitir que cada personagem/inimigo tenha habilidades únicas.
- **Fachada (Facade)** – `CombatService` esconde a complexidade do `CombateEngine` e dos turnos.
- **Repositório implícito** – o `DbContext` atua como repositório; nenhuma camada adicional foi criada para manter a simplicidade.
- **Injeção de Dependência manual** – as dependências são passadas via construtor ou instanciadas diretamente, mantendo o projeto acessível para um cenário de console.

### Persistência

O Entity Framework Core em modo Code-First gerencia o esquema do banco de dados. As migrações garantem que a estrutura esteja sempre sincronizada com as classes `Models`. SQLite foi escolhido por ser leve, portátil e não exigir servidor.

### Considerações sobre o Combate

O subsistema de combate foi extraído para `Core/Combat` com as seguintes responsabilidades:
- `CombateEngine` – orquestra o loop de batalha.
- `TurnoJogador` e `TurnoInimigo` – controlam as ações de cada lado.
- `CombateUI` – gerencia toda a saída textual e entrada durante o combate.

Essa separação permitiu testar a lógica de turnos independentemente da interface e facilitou a correção de bugs relacionados a status e cálculos de dano.







---

## Desenvolvimento Futuro

- **Ascensão de Personagens:** Cópias repetidas de um mesmo personagem concederão bônus de atributos ou habilidades.
- **Expansão de Conteúdo:** Novos inimigos e personagens com mecânicas distintas.
- **Interface Melhorada:** Possível migração para uma interface gráfica simples (Windows Forms ou Terminal.Gui).
