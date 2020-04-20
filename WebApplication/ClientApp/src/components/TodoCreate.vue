<script>
import templates from './templates'

var template = templates.getTemplate('TodoCreate.template.vue')
export default Object.assign({
    data() {
        return {
            description: '',
            type: ''
        }
    },
    methods: {
        openDialog() {
            this.description = '';
            this.type = '';
            this.$refs['dlgCreateTask'].open();
        },
        closeDialog() {
            this.$refs['dlgCreateTask'].close();
        },
        createTask() {
            this.closeDialog();
            this.$store.dispatch('createTask', { description: this.description, type: this.type })
        },
        getTaskTypes(param) {
            return new Promise((resolve, rejected) => {
                resolve(this.$store.getters.getTaskTypes)
            })
        }
    }
}, template);
</script>
