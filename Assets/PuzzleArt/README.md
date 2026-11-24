# Arte de puzzles y fuente Escape Room

Los modelos se reutilizan del paquete ya presente en el proyecto:
`Assets/AssetsDescargados/low-poly-modular-dungeon-sand-biome`.
No se descargaron paquetes adicionales ni se añadieron dependencias de ejecución.
Las mallas de ItemStand y Barrel se guardaron centradas y normalizadas
para ajustar su aspecto a las dimensiones existentes sin mover los objetos físicos.
La malla Pillar3 del puzzle se sustituyó por una columna octagonal cerrada,
con tapas y normales calculadas por cara, conservando su GUID y dimensiones.
Los materiales usan URP/Lit y los colores del paquete original, con detalles de bronce.

Los prefabs de `Assets/Prefabs Puzzles` conservan sus objetos interactivos,
scripts, eventos XR, colliders y bisagras. Los adornos añadidos no tienen colliders.
Los renderizadores originales se conservan para mantener las referencias de Simon Dice.
Level_1 conserva la distribución y las propiedades físicas de los objetos existentes.

## Correcciones visuales

Barrel y ItemStand contienen dos submallas: ahora ambos tienen dos materiales
asignados (madera/hierro y bronce/oro). Los materiales de barriles y pilares
se guardaron opacos y con ambas caras visibles para las superficies abiertas
de los modelos importados. Los materiales incrustados en los FBX no se modificaron;
se utilizan copias externas en esta carpeta.

Las palancas conservan las bisagras, colliders y eventos originales. Cada Mango
tiene un pomo esférico y un collar decorativo sin colliders; la escala local
compensa la escala no uniforme del mango para que el pomo se vea redondo.
Las 16 letras del puzzle de cubos están en blanco.

Después de estas correcciones, se compararon 808 propiedades de transformaciones,
física y comportamiento: todas permanecieron iguales. En Level_1 no quedaron
renderizadores con menos materiales que submallas, ni scripts faltantes.

## Fuente y Git

Los tres assets SDF de Escape Room contenían marcadores de conflictos de Git.
Se repararon conservando sus GUID y se regeneraron como atlas estáticos de 1024 px
con 191 caracteres latinos, incluidos acentos. Liberation Sans queda como fallback.
La fuente se asignó a los textos de las escenas del juego y sus prefabs.
Se ajustaron tamaños de texto cuando el cambio de fuente provocaba desbordamientos.

`.gitattributes` trata estos atlas como unidades durante un merge: las tablas de
glifos y los píxeles no deben combinarse por líneas. Si vuelve a haber un conflicto,
elegir un atlas completo o regenerarlo desde el OTF; nunca guardar los marcadores.
Para compartir los cambios hay que incluir en el commit los `.asset`, `.meta`,
materiales, mallas, prefabs, escenas y `.gitattributes`; un push solo envía commits.

## Límites de validación

La comprobación de estructura compara posiciones, rotaciones, escalas, física
y componentes de comportamiento con el estado anterior a los cambios visuales.
Las pruebas en Play invocan los controladores existentes; no sustituyen una prueba
de agarre, rayos y mandos con el visor físico.

Resultado: 791 propiedades originales sin cambios y ningún script faltante.
Pasaron 12 comprobaciones: pista y bloqueo de estatuas resueltas, rechazo y aceptación
del teclado, texto OPEN, rechazo y aceptación del pedestal, conexiones de cubos y
palancas, victoria de Simon Dice, avance de frames y final de la animación de puerta.
Las letras y UIs de Level_1 no reportaron desbordamiento después del ajuste.

Durante Play se detectó `DllNotFoundException: UnityOpenXR` en el entorno del editor.
También había ajustes XR con conflictos anteriores; Unity reserializó archivos XR
durante Play. Se mantuvo como configuración activa el asset original
`Assets/XR/Settings/OpenXR Package Settings.asset`.
La compilación C# está correcta, pero queda pendiente validar OpenXR y la interacción
con el visor físico. No se ejecutó una compilación Android ni se hizo commit o push.

## Integración de la demo en Level_1

- GameUI es hija de la cámara del XROrigin, como en EscapeRoom. Su escala compensa
  la escala del rig y el temporizador queda dentro del campo de visión.
- Game Over y Victory usan materiales de HUD que se dibujan sobre la geometría;
  una pared no oculta los mensajes ni bloquea sus raycasts de UI.
- Se añadió XR EventSystem con XRUIInputModule, se habilitó UI en los controles
  Near-Far y se conectaron los botones Reintentar al GameTimer de la escena.
- GameTimer distingue partida activa, derrota y victoria; pausa la partida y la
  locomoción al finalizar, conservando el seguimiento de cabeza y los botones.
- Demo Goal está después de la puerta de gigi2. Solo la cabeza del jugador dentro
  de la zona, con esa puerta abierta, activa «felicidades pasaste la demo».
- Se añadieron 19 colliders al suelo visible y al plano inferior de respaldo.
- Quince barriles, cajas, patitos y calaveras adicionales tienen agarre XR. Conservan
  su posición inicial hasta agarrarlos; al soltarlos activan gravedad. Los dos
  objetos de gigi2 conservan sus agarres y las siete piezas con bisagra conservan
  sus restricciones. No se habilitó agarre para paredes ni decoración estructural.
- Los patitos y calaveras que eran hijos del primer patito se separaron conservando
  sus posiciones mundiales para que agarrar uno no arrastre a los demás.
- Se registraron explícitamente los colliders de los interactables; ImanCriptex
  desactiva su resorte durante el agarre y lo recupera al soltar.

Pruebas en Play: seguimiento del HUD al trasladar/girar el rig, suelo en cuatro
zonas, selección XR real y desplazamiento físico de una caja durante varios frames,
gravedad al soltar y apoyo en el suelo, rechazo de victoria con puerta cerrada,
victoria al entrar después de resolver el pedestal, expiración natural del contador,
exclusión entre victoria y derrota y reinicio desde ambos botones de resultado.
Se revisaron capturas de HUD, Game Over y victoria. El shader de HUD compiló sin
errores. Persiste en este editor el error de carga nativa UnityOpenXR mencionado
arriba; las pruebas no equivalen a una validación con visor y mandos físicos.

## Salas ampliadas y puerta con tres requisitos

Level_1 agrupa el escenario en `Demo Rooms`, con escala uniforme 1.20 alrededor
del punto inicial. Esto amplía el ancho y fondo un 20 % y conserva las conexiones
entre habitaciones, los anclajes de bisagras y la forma esférica de los pomos.
El rig y el HUD quedan fuera de este grupo. El temporizador subió de Y=180 a
Y=280 en su RectTransform; los paneles de resultado permanecen centrados.

Se añadieron 18 colliders sólidos a las piezas del techo y diez objetos agarrables
(seis cajas y cuatro piedras). No se aumentó la cantidad de barriles, patitos ni
calaveras que se cuentan en una pista. Las cuatro hojas conservan sus referencias,
pero tienen un tamaño manejable, collider fino, Rigidbody, agarre XR, respaldo
y copia legible por el reverso. Se pueden recoger y volver a consultar.

`FinalDoorRequirements`, en la puerta de gigi2, requiere exclusivamente:

- `SimondiceBrain.IsSolved` después de completar sus rondas configuradas.
- `ManagerCriptex.IsSolved` después de alinear las cuatro bisagras con sus ángulos.
- `PedestalController.IsSolved` después de colocar el objeto correcto.

`DoorController.OpenDoor` consulta estos requisitos incluso si lo llama el pedestal.
El controlador vuelve a comprobarlos cada frame, por lo que el orden de resolución
es libre. Colocar el objeto primero no abre la puerta ni obliga a colocarlo de nuevo.
Los estados de resolución se reinician al recargar la escena. Las otras puertas
conservan su comportamiento porque el requisito es opcional y solo está conectado
en la puerta final de Level_1.

Validación: pasaron 19 comprobaciones en Play. Se rechazaron las siete combinaciones
incompletas, se resolvieron físicamente las bisagras (0, -90, 0, 180 grados), el
trigger detectó el objeto correcto, Simon completó sus seis rondas configuradas y
la puerta abrió y terminó su animación. Se probaron techo y suelo en cuatro zonas,
el movimiento de una hoja mediante XR y los diez objetos nuevos. La victoria al
entrar y el reinicio de los tres estados desde Reintentar también pasaron.

## Paso de puerta, columnas y tarjetas rígidas

Las doce columnas `dspilar`/`dspilar2` del segundo nivel tienen MeshCollider sólido.
El marco `Pillar2Door` usa la geometría de su malla como collider; el BoxCollider
anterior también llenaba el hueco de la puerta. El CharacterController conserva
su seguimiento y altura, con radio 0.24, Skin Width 0.025 y Step Offset 0.20 para
facilitar el paso y evitar engancharse en pequeños desniveles.
El puzzle de palancas mantiene la puerta abierta después de resolverlo.

Las hojas conservan sus objetos y referencias de pistas, pero sus SpriteRenderers
y capas superpuestas quedan desactivados. La representación es una tarjeta cerrada
de 6 mm, con textura fija por ambos lados y material URP/Unlit opaco. El collider
tiene 12 mm de espesor. El agarre es individual, cinemático, suavizado y sin cambio
de escala. Al soltar, la tarjeta queda inmóvil para poder volver a consultarla;
ya no utiliza VRPropPhysics ni activa gravedad.

Pasaron ocho comprobaciones en Play: columnas sólidas, bloqueo con puerta cerrada,
apertura por las palancas, paso usando el CharacterController real, apertura
persistente al soltar una palanca, movimiento de la tarjeta mediante XR, escala
constante y posición estable tras soltarla. No se observaron errores de compilación.
La estabilidad visual en el visor físico debe confirmarse con el dispositivo.

Actualización de física de las tarjetas: conservan la malla y textura fija, pero
vuelven a utilizar VRPropPhysics. Al soltarlas se activan Rigidbody dinámico y
gravedad; también permiten lanzarlas. Permanecen en su posición inicial hasta
el primer agarre para conservar la disposición de las pistas.

## Pivotes corregidos y límites de cabeza VR

Se corrigieron los pivotes de la primera puerta y de la puerta del teclado en
Level_1, además de DoorSystem.prefab y SistemaPuzle.prefab. Cada pivote queda
sobre un borde lateral de su hoja, conservando la posición de la puerta cerrada.
El pivote de gigi2 ya estaba correcto y se conservó. Las siete bisagras de palancas
y cubos mantienen coincidentes sus anclajes.

El rig utiliza VRHeadCollisionGuard para comprobar una esfera de 12 cm alrededor
del visor, tanto en LateUpdate como después del seguimiento antes de renderizar.
Los barridos continuos frenan la posición virtual frente a paredes, puertas, techo
y suelo; permiten deslizarse junto a una pared. La corrección se aplica al origen
XR y conserva la rotación del visor. El collider corporal sigue la cabeza en XZ.
Las manos, las pistas y los controles interactivos se excluyen de estas consultas
para que sujetar un objeto no empuje al jugador. La cámara conserva su plano
cercano de 1 cm. Esto limita el movimiento virtual; no frena el movimiento físico
de la persona y debe probarse con el visor para comprobar la comodidad.

Validación en Play: diez comprobaciones de movimiento de cabeza simulado, paredes
inclinadas, desplazamiento lateral, retroceso, salto de seguimiento grande, giro
del visor, seguimiento corporal, techo, suelo y paso por puerta cerrada/abierta.
La comprobación geométrica de los tres pivotes deja desplazamientos menores que
0.01 mm respecto a sus bordes y al plano de la hoja.
