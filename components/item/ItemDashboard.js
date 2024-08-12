export class ItemDashboard extends HTMLElement {
    constructor() {
        super();
    }
    connectedCallback() {
        const isApplicable = this.getAttribute("applicable") !== null;

        this.innerHTML = `
          <item-filter></item-filter>
          <item-table></item-table>
        `;
    }
}
