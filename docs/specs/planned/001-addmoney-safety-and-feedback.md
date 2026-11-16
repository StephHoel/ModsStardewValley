# 001 - AddMoney: segurança do saldo e feedback

**Prioridade:** P1  
**Área:** AddMoney

## Contexto

O comando de adição de dinheiro não deve aceitar valores inválidos nem apresentar mensagens incorretas quando o jogador ou o estado do saldo não estiver disponível.

## Requisitos

- Validar se o jogador de destino está disponível antes de alterar o saldo.
- Aceitar somente valores positivos; rejeitar zero e valores negativos com uma mensagem útil.
- Evitar estouro de inteiro e definir o comportamento ao atingir o limite de saldo suportado pelo jogo.
- Se o jogo limitar o saldo, informar o valor efetivamente adicionado, não apenas o valor solicitado.
- Manter a análise dos argumentos e as mensagens consistentes com o sistema de localização do mod.

## Critérios de aceitação

- Testes cobrem jogador ausente, entrada malformada, zero, valor negativo, valor normal, valor muito alto e valores próximos ao limite.
- Entradas inválidas não alteram o saldo.
- O feedback corresponde exatamente à alteração realizada.
- Os usos válidos existentes do comando continuam funcionando.
