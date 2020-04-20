var req = require.context('../templates/Todo/', false, /\.vue$/)

export default {
    getTemplate(template) {
        if (req.keys().indexOf('./' + template) > -1) {
            return req('./' + template);
        }
        else {
            return require('../templates/' + template);
        }
    }
}

