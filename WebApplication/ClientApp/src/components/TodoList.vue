<script>
import templates from './templates'

var template = templates.getTemplate('TodoList.template.vue')
export default Object.assign({
    data() {
        return {
            todos: this.$store.getters.getTasks
        }
    },
    methods: {
        markCompleteTask(id) {
            this.$store.dispatch('completeTask', { id : id })
        }
    }
}, template);
</script>