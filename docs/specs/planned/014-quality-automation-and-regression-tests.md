# 014 - Todos os mods: automação de qualidade e testes de regressão

**Prioridade:** P1  
**Área:** Transversal

## Contexto

A solução aparentemente não possui um projeto dedicado a testes automatizados. Como é difícil exercitar integrações com o jogo em testes unitários comuns, os limites entre testes automatizados e verificações manuais devem ser definidos deliberadamente.

## Requisitos

- Introduzir um projeto de testes para lógica pura: validação de configurações, cálculos de tempo, mapeamento de IDs e comparação de traduções.
- Criar abstrações ou dublês restritos para dependências do GMCM e do jogo quando isso melhorar a testabilidade.
- Adicionar verificações básicas de compilação e carregamento para integrações com SMAPI e Harmony, quando viável.
- Transformar relatos históricos conhecidos, incluindo cobertura de galpões/interiores, processamento duplicado e travamentos momentâneos, em cenários de regressão, sem tratar relatos não verificados como defeitos confirmados.
- Documentar quais comportamentos exigem verificação manual dentro do jogo.

## Critérios de aceitação

- A lógica determinística central possui testes automatizados e reproduzíveis.
- Os testes são executados no CI e suas falhas impedem a validação.
- Há instruções de testes manuais para comportamentos que não podem ser cobertos de forma confiável por testes unitários.
