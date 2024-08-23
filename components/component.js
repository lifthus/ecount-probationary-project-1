import { ErpBanner } from "./common/ErpBanner.js";
import { ErpBody } from "./common/ErpBody.js";
import { MenuTitle } from "./common/MenuTitle.js";

import { FilterInputText } from "./common/filter/FilterInputText.js";
import { FilterItemDiv } from "./common/filter/FilterItemDiv.js";
import { BlueButton } from "./common/BlueButton.js";
import { GrayButton } from "./common/GrayButton.js";

import { NavBar } from "./main/NavBar/NavBar.js";
import { MenuButton } from "./main/NavBar/MenuButton.js";

import { ItemDashboard } from "./item/ItemDashboard.js";
import { ItemFilter } from "./item/ItemFilter.js";
import { ItemTable } from "./item/itemTable.js";

import { SaleDashboard } from "./sale/SaleDashboard.js";
import { SaleFilter } from "./sale/SaleFilter.js";
import { FilterDateRange } from "./common/filter/FilterDateRange.js";
import { FilterItemSelect } from "./common/filter/FilterItemSelect.js";
import { FilterSelectedItem } from "./common/filter/FilterSelectedItem.js";

/* common */
defineComponent(ErpBanner);
defineComponent(ErpBody);
defineComponent(MenuTitle);
defineComponent(BlueButton);
defineComponent(GrayButton);
/* filter */
defineComponent(FilterInputText);
defineComponent(FilterItemDiv);
defineComponent(FilterDateRange);
defineComponent(FilterItemSelect);
defineComponent(FilterSelectedItem);
/* main */
defineComponent(NavBar);
defineComponent(MenuButton);
/* sale */
defineComponent(SaleDashboard);
defineComponent(SaleFilter);
/* item */
defineComponent(ItemDashboard);
defineComponent(ItemFilter);
defineComponent(ItemTable);

function defineComponent(component) {
    /**
     * @type {string}
     */
    const componentName = component.name;
    const nameComponents = componentName.split(/(?=[A-Z])/);
    const lowerNameComponents = nameComponents.map((nameComponent) => nameComponent.toLowerCase());
    const tagName = lowerNameComponents.join("-");
    customElements.define(tagName, component);
}
