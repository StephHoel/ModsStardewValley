# 007 - Configure Machine Speed: ciclo de vida do GMCM

**Prioridade:** P2  
**Área:** Configure Machine Speed

## Contexto

A integração com o GMCM aparentemente é registrada a partir de `RenderedActiveMenu`, evento que pode ocorrer repetidamente e provocar trabalho redundante de registro.

## Requisitos

- Registrar a integração com o GMCM uma única vez no evento de ciclo de vida apropriado, normalmente `GameLaunched`.
- Manter atualizados os callbacks de leitura e gravação da configuração e persistir as alterações pelo fluxo já estabelecido.
- Tratar a ausência do GMCM sem gerar erros.
- Evitar inscrições duplicadas em eventos ou registros repetidos.

## Critérios de aceitação

- O registro acontece uma única vez por inicialização do jogo.
- Alterações feitas no GMCM entram em vigor e são preservadas após reiniciar o jogo.
- O mod funciona mesmo quando o GMCM não está instalado.
