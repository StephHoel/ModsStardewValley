# 012 - Solução: portabilidade de compilação e CI

**Prioridade:** P1  
**Área:** Solução / CI

## Contexto

O arquivo `Utils.csproj` aparentemente contém um HintPath absoluto para a instalação do jogo via Steam. Também foi relatado que o workflow `.github/workflows/upload.yaml` referencia uma branch `niam`, um caminho de compactação `src/` possivelmente inexistente e textos provisórios de lançamento.

## Requisitos

- Remover caminhos absolutos específicos de uma máquina; utilizar ModBuildConfig ou um caminho configurável para a instalação do jogo.
- Adicionar validações de CI para pull requests e pushes: restauração de dependências, compilação e testes relevantes.
- Separar validação da automação de publicação/envio; tarefas de lançamento devem executar somente mediante gatilhos explícitos.
- Gerar um pacote para cada mod a partir da saída real do respectivo projeto e validar os arquivos de manifesto necessários.
- Substituir textos provisórios por metadados específicos do projeto e falhar com mensagens claras quando faltar configuração.

## Critérios de aceitação

- Um clone limpo compila no CI sem depender do caminho Steam de uma máquina específica.
- O CI valida todos os projetos suportados.
- Os arquivos de lançamento contêm os arquivos pretendidos do mod, sem incluir diretórios de código-fonte não relacionados.
- Não há valores provisórios nos workflows ativos de publicação.
