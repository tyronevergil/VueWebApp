import Vue from 'vue'
import Vuex from 'vuex'
import VueMaterial from 'vue-material'
import 'vue-material/dist/vue-material.css'

import App from './App.vue'
import { storeDefinition, startHub } from './store'

Vue.use(Vuex)
Vue.use(VueMaterial)

const store = new Vuex.Store(storeDefinition);
startHub(store);

new Vue({
    store,
    render: h => h(App)
})
.$mount("#app-container")
.$material.registerTheme('default', {
    primary: {
        color: 'blue-grey',
        hue: 800
    }
})