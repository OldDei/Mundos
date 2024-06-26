#version 330 core

layout (location = 0) in vec3 aPos;

out vec4 vertexColor;

uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;
uniform vec4 defaultColor;

void main()
{
    vertexColor = defaultColor;
    gl_Position = vec4(aPos, 1.0) * model * view * projection;
}