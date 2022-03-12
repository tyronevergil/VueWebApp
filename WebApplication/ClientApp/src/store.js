import axios from 'axios'
import * as signalR from '@aspnet/signalr'

function uuidv4() {
  return ([1e7]+-1e3+-4e3+-8e3+-1e11).replace(/[018]/g, c =>
    (c ^ crypto.getRandomValues(new Uint8Array(1))[0] & 15 >> c / 4).toString(16)
  )
}

function startHub(store) {

    var hubConnection = new signalR.HubConnectionBuilder()
        .withUrl('/dataHub')
        .build();

    hubConnection.start()
        .then(() => {

            console.log('Hub connected!')

            function loadData() {
                axios.get('/api/Query/Tasks')
                    .then(function (response) {
                        if (response.data) {
                            var d = [0, store.state.Tasks.length].concat(response.data)
                            store.state.Tasks.splice.apply(store.state.Tasks, d)
                        }
                    });

                axios.get('/api/Query/TaskTypes')
                    .then(function (response) {
                        if (response.data) {
                            var d = [0, store.state.TaskTypes.length].concat(response.data)
                            store.state.TaskTypes.splice.apply(store.state.TaskTypes, d)
                        }
                    });
            }

            hubConnection.on('task.created', (result) => {
                var data = JSON.parse(result);
                axios.get('/api/Query/Tasks/' + data.Id)
                    .then(function(response) {
                        if (response.data) {
                            store.commit('addTask', response.data)
                        }
                    });
            })

            hubConnection.on('task.completed', (result) => {
                var data = JSON.parse(result);
                axios.get('/api/Query/Tasks/' + data.Id)
                    .then(function(response) {
                        if (response.data) {
                            store.commit('editTask', response.data)
                        }
                    });
            })

            hubConnection.on('tasktype.created', (result) => {
                var data = JSON.parse(result);
                axios.get('/api/Query/TaskTypes/' + data.Id)
                    .then(function(response) {
                        if (response.data) {
                            store.commit('addTaskType', response.data)
                        }
                    });
            })

            hubConnection.on('event.replay', () => {
                console.log("Replay...");
                loadData();
            })

            hubConnection.onclose(() => {
                console.log("Hub disconnected.")
                setTimeout(() => {
                    console.log("Hub reconnecting...")
                    startHub(store);
                }, 5000)
            })

            loadData();
        })
        .catch(() => {
            console.log("Hub error connecting.")
            setTimeout(() => {
                console.log("Hub reconnecting...")
                startHub(store);
            }, 5000)
        })

}

const storeDefinition = {
    state: {
        Tasks: [],
        TaskTypes: []
    },
    getters: {
        getTasks: state => {
            return state.Tasks;
        },
        getTaskTypes: state => {
            return state.TaskTypes;
        }
    },
    mutations: {
        addTask (state, task) {
            state.Tasks.push(task)
        },
        editTask (state, task) {
            var r = state.Tasks.filter(t => t.id == task.id);
            if (r.length) {
                r[0].description = task.description;
                r[0].type = task.type;
                r[0].completed = task.completed;
            }
        },
        addTaskType (state, taskType) {
            state.TaskTypes.push(taskType);
        }        
    },
    actions: {
        createTask (context, payload) {
            payload.transactionId = uuidv4();
            axios.post('/api/Command/TaskCreate', payload);
        },
        completeTask (context, payload) {
            payload.transactionId = uuidv4();
            axios.post('/api/Command/TaskComplete', payload);
        }
    }
}

export {
    storeDefinition,
    startHub
}