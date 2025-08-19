# Shyyxn Unlock — Personalização + Módulos de Loaders

Este overlay adiciona:
- Branding básico ("Shyyxn Unlock")
- Gerenciador de Loaders (importar/arrastar .mbn/.elf/.bin e opcionais rawprogram/patch XML)
- Persistência em `Modules/loaders.json`
- Integração com as rotinas existentes de flash para Qualcomm/HiSilicon (suporte básico)
- Estrutura para MediaTek (placeholder)

## Como compilar
1. Abra `HuaweiUnlock.sln` no Visual Studio 2019+.
2. No projeto **HuaweiUnlock**, referencie `System.Web.Extensions` (para JavaScriptSerializer), se necessário.
3. Build **x86**. Execute.

## Como usar
- No topo da janela, clique em **Modules → Gerenciar Loaders**.
- Clique em **Importar Loader** ou arraste arquivos `.mbn/.elf/.bin` sobre a janela.
- Informe `Vendor`, `Model` e `Chipset`. Opcionalmente selecione `rawprogram0.xml` e `patch0.xml`.
- Os arquivos são copiados para `Modules/Loaders/<Chipset>/<Vendor>/...` e o manifesto é salvo em `Modules/loaders.json`.

## Próximos passos sugeridos
- Mapear partição FRP por fabricante e criar ações rápidas (Erase/Write) ligadas aos loaders.
- Adicionar login online (Supabase) bloqueando uso sem assinatura.
- Exibir detecção automática de chipset (porta) e sugerir loader compatível.