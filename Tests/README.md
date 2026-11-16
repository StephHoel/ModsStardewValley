# Estratégia de testes

## Testes automatizados

Execute os testes com:

```bash
dotnet test Tests/TranslationSummary.Tests.csproj --configuration Release
```

O projeto cobre o comportamento determinístico do relatório de traduções: resolução de status ausentes, coleta de avisos e geração do Markdown. A validação de sintaxe dos arquivos JSON e dos campos obrigatórios dos manifestos é executada pelo workflow de CI.

## Verificações manuais dentro do jogo

Os cenários abaixo dependem do runtime de Stardew Valley/SMAPI e devem ser executados em um perfil de teste com backup do salvamento.

### AddMoney
- Configurar valor zero e valor negativo; pressionar a tecla não deve alterar o saldo e deve exibir o aviso.
- Configurar valor positivo e confirmar que o saldo aumenta pelo valor esperado.
- Testar saldo próximo ao limite de inteiro e valor solicitado acima da capacidade restante; o saldo não deve sofrer overflow e o feedback deve refletir o valor efetivamente adicionado.
- Testar com o mundo não carregado e confirmar que nenhuma alteração ocorre.

### AutoWater
- Testar a tecla padrão e uma tecla personalizada.
- Digitar no chat e interagir com menus/campos de texto; a tecla não deve alternar o recurso.
- Alternar o estado, abrir o GMCM, redefinir, salvar e reiniciar o jogo; interface, configuração e comportamento devem permanecer sincronizados.
- Validar fazenda, estufa, galpões, interiores de construções e localidades personalizadas.
- Em multiplayer, confirmar que somente o anfitrião executa a rega.

### Configure Machine Speed
- Colocar várias máquinas em processamento e alternar interações entre elas; o estado de coleta de uma máquina não deve afetar outra.
- Validar Crystalarium e Cask, incluindo progressão de qualidade, retirada do produto e reinício do ciclo.
- Confirmar que máquinas dependentes de DayUpdate e máquinas excluídas continuam com o comportamento normal.
- Testar arquivos de configuração atuais e configurações antigas com nomes legados, verificando os avisos para entradas não resolvidas.
- Repetir os cenários em multiplayer como anfitrião e como farmhand.

### Compatibilidade e publicação
- Compilar a solução em uma instalação limpa com Stardew Valley detectado automaticamente ou com `STARDEW_VALLEY_PATH` configurado.
- Conferir os ZIPs gerados em `_releases` e verificar se cada pacote contém somente os arquivos do mod correspondente.
- Executar o workflow de publicação manual somente após revisar versão, nome e descrição enviados ao Nexus Mods.

## Limites conhecidos

Os testes automatizados não substituem os cenários manuais de integração com o jogo. Mods de terceiros, localidades personalizadas e diferenças de versão devem ser verificados em instalações representativas antes de uma publicação.
