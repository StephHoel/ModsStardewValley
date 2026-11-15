# 004 - AutoWater: estado após redefinição no GMCM

**Prioridade:** P1  
**Área:** AutoWater

## Contexto
A redefinição das configurações pelo GMCM aparentemente substitui a instância do objeto de configuração. Callbacks ou closures podem continuar referenciando a instância anterior.

## Requisitos
- Garantir que todos os callbacks do GMCM leiam e alterem o objeto de configuração atual.
- Reconstruir ou atualizar os callbacks com segurança quando a configuração for redefinida ou recarregada.
- Persistir os valores redefinidos pelo fluxo normal de gravação da configuração.
- Evitar referências obsoletas e inscrições duplicadas em eventos.

## Critérios de aceitação
- A redefinição restaura os valores padrão documentados tanto na interface quanto durante a execução.
- Salvar e reiniciar o jogo preserva os valores redefinidos.
- Alterações feitas depois da redefinição continuam afetando o comportamento do mod.
