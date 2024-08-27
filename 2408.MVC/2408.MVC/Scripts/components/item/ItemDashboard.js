export class ItemDashboard extends HTMLElement {
    constructor() {
        super();
    }
    connectedCallback() {
        this.innerHTML = `
          <item-filter></item-filter>
          <item-table></item-table>
        `;
    }
}
