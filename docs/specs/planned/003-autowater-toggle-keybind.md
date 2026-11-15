# 003 - AutoWater: tecla de alternância

**Prioridade:** P1  
**Área:** AutoWater

## Contexto
O GMCM disponibiliza uma tecla de atalho (relatada como `O` por padrão) cuja descrição promete alternar a ativação do AutoWater. A análise estática não identificou um manipulador de entrada atual que altere `IsActive`; é necessário verificar o comportamento durante a execução do jogo.

## Requisitos
- Verificar se a tecla configurada realmente ativa e desativa o AutoWater.
- Caso o comportamento não exista, implementar o manipulador ou corrigir a configuração e sua descrição para não prometer uma funcionalidade indisponível.
- Respeitar teclas personalizadas e não processar a entrada enquanto o jogador estiver digitando em um campo de texto.
- Exibir feedback quando apropriado e evitar alternâncias repetidas causadas pela repetição automática da tecla.

## Critérios de aceitação
- A tecla padrão e as teclas personalizadas funcionam.
- Digitar no chat, em menus ou em campos de texto não alterna o recurso acidentalmente.
- O estado em execução e o estado exibido na configuração permanecem sincronizados.
