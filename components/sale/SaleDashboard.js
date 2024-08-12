export class SaleDashboard extends HTMLElement {
    constructor() {
        super();
    }
    connectedCallback() {
        this.innerHTML = `
          <sale-filter></sale-filter>
          <sale-table></sale-table>
        `;
    }
}
