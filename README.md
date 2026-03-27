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
- Essas classes são POCOs (*Plain Old CLR Object*) e não contêm comportamento de negócio, criadas apenas para carregar os dados.

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
Contém o loop principal, o menu e a interação com o usuário. Instancia os serviços e realiza a implementação de injeção de dependência manual (*Poor Man's DI*) para desacoplamento de serviços.

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


## Sistema de Gacha: Pity, Banner e Raridades

O sistema de invocação (*gacha*) do Task-u é baseado em probabilidades com mecanismos de garantia (*pity*) para equilibrar a experiência do jogador. A seguir, são detalhados os componentes que regem os sorteios.

### Raridades e Probabilidades Base

| Raridade | Nome (Código) | Probabilidade Base |
|----------|---------------|-------------------|
| 1        | Comum (C)     | 75% (números 1–750) |
| 2        | Raro (R)      | 15% (números 751–900) |
| 3        | Épico (SR)    | 9% (números 901–990) |
| 4        | Lendário (SSR)| 1% (números 991–1000) |

Os sorteios são realizados por um gerador de números aleatórios que define um valor entre 1 e 1000. A raridade obtida é determinada por faixas fixas, exceto nos casos garantidos pelo sistema de *pity*.

### Sistema de Pity

O pity é um contador que assegura a obtenção de itens de alta raridade após um número determinado de tentativas sem sucesso. Existem dois pitys independentes:

- **Pity Épico (maxPityEpic = 10):**  
  Se o jogador realizar 10 pulls consecutivos sem obter um personagem Épico (SR) ou Lendário (SSR), o décimo pull será garantidamente um Épico (ou Lendário, caso o pity de Lendário também seja acionado).

- **Pity Lendário (maxPityLeg = 100):**  
  Se o jogador realizar 100 pulls consecutivos sem obter um Lendário, o centésimo pull será garantidamente um Lendário.  
  **Soft Pity:** A partir do 75º pull sem Lendário, a chance de obtê-lo aumenta progressivamente:  
  - 75º pull: 10% + (5 * 1) = 15%  
  - 76º pull: 10% + (5 * 2) = 20%  
  - ...  
  - 99º pull: 10% + (5 * 25) = 135% (efetivamente garantido antes do hard pity).

### Evento de Sorte (Luck Event)

Ao concluir uma tarefa épica (dificuldade ≥ 6), o jogador ativa um evento de sorte que dobra a chance de obter um personagem Lendário no próximo pull. Esse efeito é consumido no primeiro pull após a ativação.

### Banner Rotativo

O banner semanal determina quais personagens têm **rate-up** (chance aumentada) dentro de suas respectivas raridades.

- A cada 7 dias (baseado no campo `LastBannerUpdate` do usuário), o banner é atualizado:
  - Um personagem Épico e um Lendário são selecionados aleatoriamente entre os disponíveis.
  - Esses personagens são salvos na tabela `Banner`.

- Durante o sorteio:
  - Quando um pull resulta em **Épico (SR)**, há 50% de chance de ser o personagem rate-up (vs. 50% para qualquer outro Épico).
  - Quando um pull resulta em **Lendário (SSR)**, há 50% de chance de ser o personagem rate-up (vs. 50% para qualquer outro Lendário).

Essa lógica mantém o banner sempre renovado e estimula o jogador a retornar semanalmente para aproveitar os rate-ups.

### Fluxo de um Pull

1. Verifica se o pity Lendário atingiu o soft pity ou o hard pity para ajustar a chance.
2. Gera um número aleatório e compara com a chance ajustada.
3. Se Lendário: chama `BannerService.LegendPull()` – decide se será rate-up (50%) ou aleatório.
4. Se Épico: chama `BannerService.EpicPull()` – mesma lógica de rate-up.
5. Se Raro ou Comum: obtém um item correspondente das tabelas `Itens`.
6. Atualiza pitys, adiciona o item/personagem ao inventário, decrementa cristais e persiste no banco.

### Observações Técnicas

- Os pitys são armazenados por usuário (`User.PityLeg` e `User.PityEpic`) e são resetados quando um pull da raridade correspondente é obtido.
- O cálculo de soft pity é dinâmico: `chance = legChance + (5 * (pityLeg - 74))` para pityLeg ≥ 75.
- O banner é recalculado apenas quando a data da última atualização ultrapassa 7 dias, garantindo que o mesmo banner permaneça ativo durante a semana.
- O sistema utiliza `EF.Functions.Random()` no banco para selecionar itens/personagens aleatórios quando o rate-up não é escolhido.




---

## Desenvolvimento Futuro

- **Ascensão de Personagens:** Cópias repetidas de um mesmo personagem concederão bônus de atributos ou habilidades.
- **Expansão de Conteúdo:** Novos inimigos e personagens com mecânicas distintas.
- **Interface Melhorada:** Possível migração para uma interface gráfica simples (Windows Forms ou Terminal.Gui).
