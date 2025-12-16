## Caso 1 — Envio de notificação de status do agendamento de consulta

- **Objetivo:** Notificar o paciente sobre a mudança de status de sua consulta.
- **Contrato:** Exibir no console uma mensagem contendo nome do paciente, tipo da consulta, data/hora da consulta e status atual.
- **Implementação A:** Para agendamento cancelado, exibe mensagem em fundo vermelho com texto branco.
- **Implementação B:** Para agendamento atrasado, exibe mensagem em fundo amarelo com texto preto.  
- **Política:** Usar Implementação A quando o status for "cancelado" e usar Implementação B quando o status for "atrasado".
- **Risco/Observação:** Cores podem não ser visíveis em terminais sem suporte a ANSI, comprometendo a clareza.


## Caso 2 — Envio de notificação de agendamento realizado

- **Objetivo:** Confirmar o registro de um novo agendamento no sistema.
- **Contrato:** Exibir uma mensagem de confirmação contendo: nome do paciente, tipo da consulta, data/hora e status atual.
- **Implementação A:** Para agendamento com sucesso, exibe mensagem com fundo verde e texto branco.
- **Implementação B:** Para agendamento de retorno com sucesso, exibe mensagem com fundo verde, texto branco e um ícone de "🔄" antes dos dados.
- **Política:** Usar Implementação A quando for primeira consulta e Implementação B quando for consulta de retorno.
- **Risco/Observação:** Ícones podem não renderizar corretamente em alguns ambientes.