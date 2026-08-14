# Sistema de Combate – Task-u

## Visão Geral

O combate é estruturado como um RPG de turnos alternados definidos pela **velocidade** dos personagens e inimigos. O jogador controla até dois personagens simultaneamente contra um inimigo gerado dinamicamente. Cada batalha exige planejamento no uso de habilidades, itens e gerenciamento de recursos.

---

## Componentes Principais

A lógica de combate está organizada na pasta `Core/Combat` e é composta por quatro classes principais:

| Classe | Responsabilidade |
|--------|------------------|
| `CombateEngine` | Orquestra o loop da batalha,  gerenciando a fila de ações pelo AV e finalizando o combate. |
| `TurnoJogador` | Gerencia a sequência de ações do jogador para cada personagem vivo. |
| `TurnoInimigo` | Gerencia a sequência de ações do inimigo. |
| `CombateUI` | Responsável por toda a exibição e entrada do usuário durante o combate. |

---

## Fluxo da Batalha

O `CombateEngine` executa o seguinte loop:

1. **Inicialização**  
   - Exibe a introdução do inimigo (`CombateUI.Chamada`).  
   - Restaura HP máximo dos personagens, define `chanceAlvo` inicial (50% cada) e o aliado.  
   - Reseta `AvAtual` para cada entidade (calculado como `10000 / SpeedTotal()`).  
   - Define `tempoAcumulado = 0` e `tempoLimite = 100`.

2. **Seleção do Próximo Ação**  
   - Obtém a entidade (personagem ou inimigo) com o menor `AvAtual` (entre os vivos).  
   - Subtrai esse AV de todos os outros (para manter a fila).  
   - Acumula o tempo em `tempoAcumulado`.  
   - Se a entidade for um personagem, executa `TurnoJogador.turno()`.  
   - Se for o inimigo, executa `TurnoInimigo.turno()`.  
   - Após a ação, o AV da entidade que agiu é resetado para `10000 / SpeedTotal()`.

3. **Verificação de Rodada Global**  
   - Se `tempoAcumulado >= 100`, todos os buffs/debuffs (Atk, Mod, Speed, Res) são resetados, `tempoAcumulado` é zerado e o `turnoAtual` (rodada) é incrementado.

4. **Verificação de Fim**  
   - Se a equipe ou o inimigo tiver HP ≤ 0, o combate termina e as recompensas são aplicadas.

---

## Equipe e Personagens

### Composição
- Até dois personagens podem estar ativos simultaneamente, definidos pelos slots `Slot1_PersonagemAtivo` e `Slot2_PersonagemAtivo` do usuário.

### Estado
Cada personagem possui os seguintes atributos dinâmicos:
- `HpAtual` – vida atual (não ultrapassa `HpMax`).
- `Shield` – absorve dano antes do HP.
- `BuffAtk` – bônus temporário no ataque.
- `BuffMod` – bônus temporário no modificador.
- `BuffSpeed` – bônus temporário na velocidade.
- `debuffRes` – redutor temporário de resistência (multiplicador).
- `TurnoStun` – número de turnos atordoado (impede ações).  
- `TurnoSilence` – número de turnos silenciado (impede habilidades).  
- `Blinded` – (booleano) impede o ataque básico no turno atual.
- `AvAtual` – valor atual de avanço (usado na fila de ação).

### Passivas
- **Jogador:** Executada no início do turno do personagem (`PersonagemBase.Passiva()`).
- **Inimigo:** Executada antes da habilidade e ataque do inimigo (`InimigoBase.Passiva(User)`).

### Habilidades
- Cada personagem possui uma habilidade especial (`Habilidade()`), que pode ser usada uma vez por turno, a menos que esteja silenciado (`Silenced`).
- Habilidades podem causar dano, curar, aplicar status ou conceder buffs/debuffs.

### Itens Equipados
- No início do turno do personagem, se houver um item equipado, seu método `Effect(personagem)` é executado automaticamente (ex.: regeneração passiva, bônus de atributo).

---

## Turno do Jogador

Para cada personagem da equipe que ainda está vivo e tem o menor AV:

1. **Verificação de Stun**  
   Se `Stuneed` for `true`, o inimigo perde o turno e o flag é resetado.

2. **Exibição do Estado**  
   A interface mostra o status da equipe e do inimigo  e a **ordem prevista das próximas ações** (simulação com AV).

3. **Aplicação de Efeitos**  
   `aplicarEfeitos()` é chamado para decrementar contadores de stun, silence, entre outros efeitos de estado.

4. **Execução da Passiva**  
   A passiva do personagem é ativada.

5. **Menu de Ações**  
   O jogador escolhe entre as opções (cada ação pode ser usada apenas uma vez por turno):
   - **1 – Ataque Básico:** Causa dano ao inimigo baseado em `Damage()`.  
   - **2 – Habilidade Especial:** Executa `Habilidade()`. Bloqueada se `TurnoSilence > 0`.  
   - **3 – Item:** Exibe lista de itens consumíveis; ao selecionar, o efeito é aplicado e o item é removido do inventário.  
   - **4 – Encerrar Turno:** Finaliza as ações do personagem.

6. **Fim do Turno**  
    Os flags `Silenced` e `Blinded` são resetados para `false`.

---

## Turno do Inimigo

Quando o inimigo tem o menor AV:

1. **Verificação de Stun**  
   Se `Stuneed` for `true`, o inimigo perde o turno e o flag é resetado.

2. **Aplicação de Efeitos**  
   `aplicarEfeitos()` é chamado.

3. **Execução da Passiva**  
   A passiva do inimigo é ativada.

4. **Habilidade**  
   Se não estiver silenciado, o inimigo pode usar sua habilidade especial, baseada em `HabilidadeChance` (probabilidade definida por inimigo). Habilidades inimigas podem causar dano, aplicar status, etc.

5. **Ataque**  
   O inimigo causa dano a um alvo. O alvo é selecionado usando pesos de `chanceAlvo` dos personagens vivos. Quanto maior o peso, maior a chance de ser atacado.

6. **Fim do Turno**  
   O flag `Silenced` é resetado para `false`.

---

## Status e Efeitos

| Efeito | Descrição |
|--------|-----------|
| **Stun** | Impede qualquer ação no turno. O flag `Stuneed` é resetado ao final do turno |
| **Silence** | Impede o uso de habilidades, mas permite ataques básicos e itens. O flag é resetado ao final do turno. |
| **Blind** | Impede o ataque básico, mas permite habilidades e itens. Resetado ao final do turno. |
| **Shield** | Absorve dano antes do HP. O dano excedente atinge o HP. |
| **BuffAtk** | Aumento temporário no dano do ataque básico (resetado a cada rodada global). |
| **BuffMod** | Aumento temporário no modificador, afetando habilidades e cálculos de cura (resetado a cada rodada global). |
| **BuffSpeed** | Aumento temporário na velocidade (resetado a cada rodada global). |
| **debuffRes** | Redução temporária de resistência (resetado a cada rodada global). |

---

## Cálculo de Dano e Cura

- **Dano do jogador:**  
  `AtkTotal()` = ATK base + BuffAtk + (bônus de item equipado se `Atr = 2`).  
  Habilidades ou passivas podem modificar esse valor.

- **Dano do inimigo:**  
  `Damage()` = ATK base + BuffAtk (podendo ser modificado por habilidades).

- **Tomar dano:**  
  `tomarDano()` reduz o `Shield` primeiro; o excesso reduz o `HpAtual`.  
  Se o `Shield` absorver completamente o dano, uma mensagem específica é exibida.

- **Cura:**  
  `curar()` aumenta o `HpAtual` até o limite de `HpMax`.

---

## Alvos e Agressividade

- **Seleção de alvo do jogador:**  
  O jogador sempre ataca o inimigo (não há escolha de alvo para ataques básicos ou habilidades ofensivas, a menos que a habilidade especifique). Habilidades de suporte podem mirar aliados.

- **Seleção de alvo do inimigo:**  
  Baseada no atributo `chanceAlvo` de cada personagem. O inimigo sorteia um alvo ponderado pelos pesos.  
  - Inicialmente, todos têm 50% de chance.  
  - Habilidades podem alterar esses pesos temporariamente (ex.: aumentar a chance de um personagem ser atacado).

---

## Vitória e Recompensas

Quando o inimigo é derrotado:

1. Exibe a mensagem de morte (`DeathQuote` do inimigo).
2. O jogador recebe cristais (`CrystalDrop`).
3. Se `ItemDropId` for definido, o item é adicionado ao inventário.
4. O campo `DerrotouInimigo` do usuário é marcado como `true`.
5. O sistema de Aventura é chamado para gerar o próximo inimigo.

Se a equipe for derrotada:
- A mensagem de derrota é exibida e o jogador retorna ao menu principal.

---

## Geração de Inimigos

O inimigo enfrentado é determinado pelo `AdventureService` com base em duas situações:

### Regeneração Diária
No primeiro login após a meia-noite, um novo inimigo é gerado com as seguintes probabilidades:

| Raridade | Chance |
|----------|--------|
| Comum (1) | 50% |
| Raro (2)  | 30% |
| Épico (3) | 16% |
| Lendário (4)| 4% |

Após definir a raridade, um inimigo específico é sorteado aleatoriamente entre os disponíveis no banco.

### Evolução Pós-Derrota
Quando o jogador derrota um inimigo:

- Se derrotado tinha raridade **1 ou 2**: novo inimigo aumenta em 0 ou 1 (50% cada).
- Se derrotado era **3 (Épico)**: 10% de chance de evoluir para Lendário, 90% de permanecer Épico.
- Se derrotado era **4 (Lendário)**: raridade reinicia para 1 (Comum).

Em todos os casos, o inimigo é sorteado aleatoriamente dentro da raridade resultante.

---

## Implementação Técnica

- A lógica de combate está em `Core/Combat`, separada da UI e dos serviços de negócio.
- O `CombateEngine` recebe os seguintes parâmetros:  
  `(User user, InventarioServices inventario, InimigoBase inimigo, List<PersonagemBase> equipe, AppDbContext context, AdventureService adventure)`
- Durante o combate, as alterações são feitas diretamente nos objetos em memória.
- Ao final, o contexto persiste as mudanças (cristais, itens, estado do inimigo).
- A validação de entrada é centralizada em `CombateUI.EscolhaJogador`.

---

Este documento cobre os detalhes internos do combate que implementei. Para uma visão geral do jogo, consulte o [README](../README.md).
